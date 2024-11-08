using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.CoreConfigs;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.Application.Services.Impls.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IRedisCache _redisCache;
        private readonly IExecutionContext _executionContext;
        private readonly IDateService _dateService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<Resources> _localizer;

        public AuthService(
            IAuthRepository repository,
            IConfiguration configuration,
            IRedisCache redisCache,
            IExecutionContext executionContext,
            IDateService dateService,
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer
        )
        {
            _repository = repository;
            _configuration = configuration;
            _redisCache = redisCache;
            _executionContext = executionContext;
            _dateService = dateService;
            _serviceProvider = serviceProvider;
            _localizer = localizer;
        }

        public bool CheckPermission(ActionExponent[] exponents)
        {
            for (int i = 0; i < exponents.Length; i++)
            {
                var action = AuthUtility.FromExponentToPermission((int)exponents[i]);
                if (!AuthUtility.ComparePermissionAsString(_executionContext.Permission, action))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckPermission(ActionExponent exponent)
        {
            return CheckPermission(new ActionExponent[] { exponent });
        }

        public bool CheckHasAnyPermission(ActionExponent[] exponents)
        {
            for (int i = 0; i < exponents.Length; i++)
            {
                var action = AuthUtility.FromExponentToPermission((int)exponents[i]);
                if (AuthUtility.ComparePermissionAsString(_executionContext.Permission, action))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GenerateAccessTokenAsync(User user, string permission, IEnumerable<Role> roles, long selectedBranchId = default, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>();
            var secretKey = Encoding.UTF8.GetBytes(_configuration["Auth:JwtSettings:Key"]);
            var symmetricSecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(secretKey);
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricSecurityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            var roleStrings = JsonConvert.SerializeObject(roles.Select(r => new { r.Code, r.Name }), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var isCustomer = roles.Any(r => r.Code == RoleCodeConstant.CUSTOMER);
            var accountType = isCustomer ? AccountType.Customer : AccountType.Employee;
            var now = _dateService.GetClientTime();

            // add claims
            if (user.IsCustomer == true)
            {
                claims.Add(new Claim(ClaimConstant.IS_SA, "false"));
                claims.Add(new Claim(ClaimConstant.BRANCH_ID, "0"));
                claims.Add(new Claim(ClaimConstant.BRANCH_IDS, "[0]"));
            }
            else
            {
                var branchIds = user.UserBranches.Select(x => x.BranchId);

                claims.Add(new Claim(ClaimConstant.IS_SA, (roles.Any(r => r.Code == RoleCodeConstant.SUPER_ADMIN)).ToString()));
                claims.Add(new Claim(ClaimConstant.BRANCH_ID, branchIds.First(x => selectedBranchId == 0 || x == selectedBranchId).ToString()));
                claims.Add(new Claim(ClaimConstant.BRANCH_IDS, JsonConvert.SerializeObject(branchIds)));
            }

            claims.Add(new Claim(ClaimConstant.USER_ID, user.Id.ToString()));
            claims.Add(new Claim(ClaimConstant.USERNAME, user.Username ?? ""));
            claims.Add(new Claim(ClaimConstant.FULL_NAME, user.Name ?? ""));
            claims.Add(new Claim(ClaimConstant.ROLES, roleStrings));
            claims.Add(new Claim(ClaimConstant.PERMISSION, permission));
            claims.Add(new Claim(ClaimConstant.ACCOUNT_TYPE, ((int)accountType).ToString()));
            claims.Add(new Claim(ClaimConstant.IP_ADDRESS, AuthUtility.TryGetIP(_executionContext.HttpContext.Request)));
            claims.Add(new Claim(ClaimConstant.CREATE_AT, now.ToString()));
            claims.Add(new Claim(ClaimConstant.AUTHORS_MESSAGE, "Contact for work: 0859-26-1203; Facebook: https://www.facebook.com/tam.tung.92754")); 

            var expires = now.AddSeconds(AuthConfig.TokenTime);
            var securityToken = new JwtSecurityToken(
                    issuer: _configuration["Auth:JwtSettings:Issuer"],
                    audience: _configuration["Auth:JwtSettings:Issuer"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
                );

            var accessToken = tokenHandler.WriteToken(securityToken);

            /**
             * Save token vào redis
             * Nếu chỉ cho phép online trên 1 thiết bị: revoke token cũ, save token mới
             * Nếu cho phép online trên nhiều thiết bị: update token
             */
            List<AppToken> tokens;

            var appToken = new AppToken
            {
                Value = accessToken,
                Expires = expires,
                Status = TokenStatus.Ok
            };

            if (InfrastructureConfiguration.IsSingleDevice)
            {
                tokens = new List<AppToken> { appToken };
            }
            else
            {
                tokens = await GetLiveAccessTokensOfUserAsync(user.Id, cancellationToken);
                tokens = tokens.Where(x => x.Expires >= now).ToList();
                tokens.Add(appToken);
            }

            await StoreAccessTokensAsync(user.Id, tokens, cancellationToken);

            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            return Utility.RandomString(128);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken)
        {
            return await _repository.GetRefreshTokenAsync(value, ownerId, cancellationToken);
        }

        public async Task RevokeAccessTokenAsync(long userId, string accessToken, CancellationToken cancellationToken)
        {
            var tokens = await GetLiveAccessTokensOfUserAsync(userId, cancellationToken);
            var token = tokens.FirstOrDefault(t => t.Value == accessToken);

            if (token == null || token.Status != TokenStatus.Ok)
            {
                return;
            }
            token.Status = TokenStatus.LoggedOut;

            await StoreAccessTokensAsync(userId, tokens, cancellationToken);
        }

        public async Task ForceLogoutAsync(long userId, CancellationToken cancellationToken)
        {
            var tokens = await GetLiveAccessTokensOfUserAsync(userId, cancellationToken);
            if (!tokens.Any())
            {
                return;
            }

            foreach (var token in tokens)
            {
                token.Status = TokenStatus.ForceLogout;
            }
            await StoreAccessTokensAsync(userId, tokens, cancellationToken);

            //var socketService = _serviceProvider.GetRequiredService<ISocketService>();
            //await socketService.ForceLogout(userId, _localizer["account_has_been_forced_to_logout"], cancellationToken);
        }

        public string GetPermission(User user)
        {
            var roles = user.UserRoles.Select(u => u.Role);
            var sa = roles.FirstOrDefault(x => x.Code.Equals(RoleCodeConstant.SUPER_ADMIN));
            var admin = roles.FirstOrDefault(x => x.Code.Equals(RoleCodeConstant.ADMIN));

            if (sa != null)
            {
                var exponent = sa.RoleActions.First(x => x.Role.Code.Equals(RoleCodeConstant.SUPER_ADMIN)).Action.Exponent;
                return AuthUtility.CalculateToTalPermision(Enumerable.Range(0, exponent + 1));
            }
            else if (admin != null)
            {
                var exponent = admin.RoleActions.First(x => x.Role.Code.Equals(RoleCodeConstant.ADMIN)).Action.Exponent;
                return AuthUtility.CalculateToTalPermision(Enumerable.Range(0, exponent + 1));
            }

            var exponents = new List<int>();
            foreach (var role in roles)
            {
                exponents.AddRange(role.RoleActions.Select(x => x.Action.Exponent));
            }

            return AuthUtility.CalculateToTalPermision(exponents.Distinct());
        }

        public async Task CheckPasswordLevelAndThrowAsync(string pwd, CancellationToken cancellationToken)
        {
            var sc = await GetSystemConfigurationAsync(cancellationToken);
            var minLength = sc.PasswordMinLength ?? AuthConfig.PasswordMinLength;
            var result = AuthUtility.ValidatePassword(pwd, minLength, sc.RequiresPasswordLevel);

            if (!result.IsValid)
            {
                Log.Logger.Debug("Validate password. {Pwd} {Result}", pwd, result);
                var html = "";

                switch (result.RequiresLevel)
                {
                    case PasswordLevel.Weak:
                        html = $@"<div>
                                    <p><strong>{_localizer["account_password_must_include_the_following_elements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["account_password_contains_at_least_n_characters"], minLength)}</p>
                                 </div>";
                        break;
                    case PasswordLevel.Medium:
                        html = $@"<div>
                                    <p><strong>{_localizer["account_password_must_include_the_following_elements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["account_password_contains_at_least_n_characters"], minLength)}</p>
                                    <p><strong>Và thỏa mãn ít nhất 2 điều kiện sau: </strong></p>
                                    <p class='{(result.HasLowerCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_lowercase_letter"]}</p>
                                    <p class='{(result.HasUpperCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_uppercase_letter"]}</p>
                                    <p class='{(result.HasNumber ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_number"]}</p>
                                    <p class='{(result.HasSpecialChar ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_special_character"]}</p>
                                 </div>";
                        break;
                    case PasswordLevel.Strong:
                        html = $@"<div>
                                    <p><strong>{_localizer["account_password_must_include_the_following_elements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["account_password_contains_at_least_n_characters"], minLength)}</p>
                                    <p class='{(result.HasLowerCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_lowercase_letter"]}</p>
                                    <p class='{(result.HasUpperCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_uppercase_letter"]}</p>
                                    <p class='{(result.HasNumber ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_number"]}</p>
                                    <p class='{(result.HasSpecialChar ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["account_password_has_at_least_1_special_character"]}</p>
                                 </div>";
                        break;
                }

                throw new BadRequestException(ErrorCodeConstant.PWD_NOT_VALID, html);
            }
        }

        public async Task<IpInformation> GetIpInformationAsync(string ip, CancellationToken cancellationToken = default)
        {
            if (!AuthUtility.IsValidIpAddress(ip))
            {
                throw new CatchableException("IP không hợp lệ");
            }
            var factory = _executionContext.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient();
            var policy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(
                                   retryCount: 1,
                                   sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.PI),
                                   onRetry: (exception, timespan, context) =>
                                   {
                                       Log.Logger.Error($"Failed when try get ip address information with message {exception.Message}");
                                   }
                               );

            var info = new IpInformation();
            await policy.ExecuteAsync(async () =>
            {
                var result = await client.GetAsync($"https://ipinfo.io/{ip}?token={AuthConfig.IpInfoTokenKey}", cancellationToken);

                if (result.IsSuccessStatusCode)
                {
                    info = JsonConvert.DeserializeObject<IpInformation>(await result.Content.ReadAsStringAsync());
                }
            });

            return info;
        }

        public Task<IpInformation> GetIpInformationAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            return GetIpInformationAsync(AuthUtility.TryGetIP(httpContext.Request), cancellationToken);
        }

        public async Task<LoginResult> GetLoginResultAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(userId, cancellationToken);
            return await GetLoginResultAsync(user, cancellationToken);
        }

        public async Task<LoginResult> GetLoginResultAsync(User user, CancellationToken cancellationToken)
        {
            var roles = user.UserRoles.Select(x => x.Role);
            var result = new LoginResult
            {
                AccessToken = await GenerateAccessTokenAsync(user, GetPermission(user), roles, cancellationToken: cancellationToken),
                RefreshToken = GenerateRefreshToken(),
            };

            _executionContext.UpdateContext(result.AccessToken);

            var sc = await GetSystemConfigurationAsync(cancellationToken);

            // Save refresh token
            var refreshToken = new RefreshToken
            {
                RefreshTokenValue = result.RefreshToken,
                CurrentAccessToken = result.AccessToken,
                ExpiryDate = DateTime.Now.AddSeconds(sc.Session ?? AuthConfig.RefreshTokenTime),
            };

            _repository.UnitOfWork.BeginTransaction();
            _repository.AddRefreshToken(refreshToken);
            await _repository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<List<AppToken>> GetLiveAccessTokensOfUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            var key = BaseCacheKeys.GetAccessTokenKey(userId);
            return await _redisCache.GetAsync<List<AppToken>>(key, cancellationToken) ?? new();
        }

        private async Task StoreAccessTokensAsync(long userId, List<AppToken> tokens, CancellationToken cancellationToken = default)
        {
            var key = BaseCacheKeys.GetAccessTokenKey(userId);
            tokens = tokens.Where(x => x.Expires >= _dateService.GetClientTime()).ToList();

            await _redisCache.SetAsync(key, tokens, TimeSpan.FromSeconds(AuthConfig.TokenTime), cancellationToken: cancellationToken);
        }

        private async Task<SystemConfiguration> GetSystemConfigurationAsync(CancellationToken cancellationToken = default)
        {
            var systemConfigurationRepository = _serviceProvider.GetRequiredService<ISystemConfigurationReadRepository>();
            return await systemConfigurationRepository.GetAsync(cancellationToken);
        }

        public async Task ValidateAccessAndThrowAsync(User user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new BadRequestException(_localizer["account_not_found"]);
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new BadRequestException(_localizer["account_is_inactive"]);
            }

            if (user.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException(_localizer["account_is_banned"]);
            }

            var sc = await GetSystemConfigurationAsync(cancellationToken);
            if (sc.IsEnabledVerifiedAccount == true && user.Status == AccountStatus.UnConfirm)
            {
                throw new BadRequestException(_localizer["auth_account_has_not_been_verified"]);
            }
        }

        public void ValidateStateAndThrow(User user)
        {
            if (user == null)
            {
                throw new BadRequestException(_localizer["account_not_found"]);
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new BadRequestException(_localizer["account_is_inactive"]);
            }

            if (user.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException(_localizer["account_is_banned"]);
            }

        }

        public void ValidateStateIncludeActiveAndThrow(User user)
        {
            ValidateStateAndThrow(user);

            if (user.Status == AccountStatus.Active)
            {
                throw new BadRequestException(_localizer["account_already_actived"]);
            }
        }
    }
}
