using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Configs;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Models.Auths;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Net.Http.Headers;
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
        private readonly IAuthRepository _authRepository;
        private readonly IActionReadRepository _actionReadRepository;
        private readonly IRedisCache _redisCache;
        private readonly IExecutionContext _executionContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<Resources> _localizer;

        public AuthService(
            IAuthRepository repository,
            IActionReadRepository actionReadRepository,
            IRedisCache redisCache,
            IExecutionContext executionContext,
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer
        )
        {
            _authRepository = repository;
            _actionReadRepository = actionReadRepository;
            _redisCache = redisCache;
            _executionContext = executionContext;
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

        public async Task<string> GenerateAccessTokenAsync(GenTokenPayload payload, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>();
            var secretKey = Encoding.UTF8.GetBytes(JwtSettingsConfig.SecretKey);
            var symmetricSecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(secretKey);
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricSecurityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            var roleStrings = JsonConvert.SerializeObject(payload.Roles.Select(r => new { r.Code, r.Name }), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var accountType = AccountType.Anonymous;

            if (payload.User is Customer)
            {
                accountType = AccountType.Customer;
            }
            else if(payload.User is Employee)
            {
                accountType = AccountType.Employee;
            }
            else
            {
                accountType = AccountType.Doctor;
            }
            var now = DateTime.Now;

            // add claims
            if (payload.User is Customer)
            {
                claims.Add(new Claim(ClaimConstant.IS_SA, "false"));
                claims.Add(new Claim(ClaimConstant.IS_FA, "false"));
                claims.Add(new Claim(ClaimConstant.ZONE_ID, (0).ToString()));
                claims.Add(new Claim(ClaimConstant.FACILITY_ID, (0).ToString()));
            }
            else if (payload.User is Employee employee)
            {
                claims.Add(new Claim(ClaimConstant.IS_SA, (payload.Roles.Any(r => r.Code == RoleCodeConstant.SUPER_ADMIN)).ToString()));
                claims.Add(new Claim(ClaimConstant.IS_FA, (payload.Roles.Any(r => r.Code == RoleCodeConstant.FACILITY_ADMIN)).ToString()));
                claims.Add(new Claim(ClaimConstant.ZONE_ID, (employee.ZoneId).ToString()));
                claims.Add(new Claim(ClaimConstant.FACILITY_ID, (employee.FacilityId).ToString()));
            }

            claims.Add(new Claim(ClaimConstant.USER_ID, payload.User.Id.ToString()));
            claims.Add(new Claim(ClaimConstant.EMAIL, payload.User.Email));
            claims.Add(new Claim(ClaimConstant.FULL_NAME, payload.User.Name ?? ""));
            claims.Add(new Claim(ClaimConstant.PHONE, payload.User.Phone ?? ""));
            //claims.Add(new Claim(ClaimConstant.SHARDING, payload.User.Shard.ToString()));
            claims.Add(new Claim(ClaimConstant.ROLES, roleStrings));
            claims.Add(new Claim(ClaimConstant.PERMISSION, payload.Permission));
            claims.Add(new Claim(ClaimConstant.ACCOUNT_TYPE, ((int)accountType).ToString()));
            claims.Add(new Claim(ClaimConstant.IP_ADDRESS, AuthUtility.TryGetIP(_executionContext.HttpContext.Request)));
            claims.Add(new Claim(ClaimConstant.CREATE_AT, now.ToString()));

            var validSeconds = payload.ValidSeconds > 0 ? payload.ValidSeconds : AuthConfig.TokenTime;
            var expires = now.AddSeconds(validSeconds);
            var securityToken = new JwtSecurityToken(
                    issuer: JwtSettingsConfig.Issuer,
                    audience: JwtSettingsConfig.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
                );

            var accessToken = tokenHandler.WriteToken(securityToken);

            var tokens = await GetLiveAccessTokensOfUserAsync(payload.User.Id, cancellationToken);
            var appToken = new AppToken
            {
                Value = accessToken,
                Expires = expires,
                Status = TokenStatus.Ok
            };

            if (ApplicationConfig.IsSingleDevice)
            {
                tokens = new List<AppToken> { appToken };
            }
            else
            {
                tokens.Add(appToken);
            }

            await StoreAccessTokensAsync(payload.User.Id, tokens, cancellationToken);

            return accessToken;
        }

        public string GenerateRefreshToken(int expire)
        {
            var obj = new
            {
                Value = Guid.NewGuid(),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddSeconds(expire),
            };

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }).ToBase64Encode();
        }

        public Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken)
        {
            return _authRepository.GetRefreshTokenAsync(value, ownerId, cancellationToken);
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

            var socketService = _serviceProvider.GetRequiredService<ISocketService>();
            await socketService.ForceLogout(userId, _localizer["Account.HasBeenForcedToLogout"], cancellationToken);
        }

        public async Task FetchNewTokenAsync(long userId, string message, CancellationToken cancellationToken)
        {
            var tokens = await GetLiveAccessTokensOfUserAsync(userId, cancellationToken);
            if (!tokens.Any())
            {
                return;
            }

            foreach (var token in tokens)
            {
                token.Status = TokenStatus.FetchNew;
            }
            await StoreAccessTokensAsync(userId, tokens, cancellationToken);

            var socketService = _serviceProvider.GetRequiredService<ISocketService>();
            await socketService.AskReload(userId, message, cancellationToken);
        }

        public string GetPermission(Employee employee, List<ActionWithExcludeValue> actions)
        {
            var roles = employee.EmployeeRoles.Select(u => u.Role);
            //var sa = roles.FirstOrDefault(x => x.Code.Equals(RoleCodeConstant.SUPER_ADMIN));
            //var admin = roles.FirstOrDefault(x => x.Code.Equals(RoleCodeConstant.ADMIN));

            //if (sa != null)
            //{
            //    var exponent = sa.RoleActions.First(x => x.Role.Code.Equals(RoleCodeConstant.SUPER_ADMIN)).Action.Exponent;
            //    return AuthUtility.CalculateToTalPermision(Enumerable.Range(0, exponent + 1));
            //}
            //else if (admin != null)
            //{
            //    var exponent = admin.RoleActions.First(x => x.Role.Code.Equals(RoleCodeConstant.ADMIN)).Action.Exponent;
            //    return AuthUtility.CalculateToTalPermision(Enumerable.Range(0, exponent + 1));
            //}

            var exponents = actions.Select(a => a.Exponent);

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
                                    <p><strong>{_localizer["Account.PasswordMustIncludeTheFollowingElements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["Account.PasswordContainsAtLeastNCharacters"], minLength)}</p>
                                 </div>";
                        break;
                    case PasswordLevel.Medium:
                        html = $@"<div>
                                    <p><strong>{_localizer["Account.PasswordMustIncludeTheFollowingElements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["Account.PasswordContainsAtLeastNCharacters"], minLength)}</p>
                                    <p><strong>Và thỏa mãn ít nhất 2 điều kiện sau: </strong></p>
                                    <p class='{(result.HasLowerCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1LowercaseLetter"]}</p>
                                    <p class='{(result.HasUpperCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1UppercaseLetter"]}</p>
                                    <p class='{(result.HasNumber ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1Number"]}</p>
                                    <p class='{(result.HasSpecialChar ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1SpecialCharacter"]}</p>
                                 </div>";
                        break;
                    case PasswordLevel.Strong:
                        html = $@"<div>
                                    <p><strong>{_localizer["Account.PasswordMustIncludeTheFollowingElements"]}:</strong></p>
                                    <p class='{(result.ValidLength ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{string.Format(_localizer["Account.PasswordContainsAtLeastNCharacters"], minLength)}</p>
                                    <p class='{(result.HasLowerCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1LowercaseLetter"]}</p>
                                    <p class='{(result.HasUpperCase ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1UppercaseLetter"]}</p>
                                    <p class='{(result.HasNumber ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1Number"]}</p>
                                    <p class='{(result.HasSpecialChar ? "msg-success" : "msg-failed")}'> <span class='icon bg-contain'></span>{_localizer["Account.PasswordHasAtLeast1SpecialCharacter"]}</p>
                                 </div>";
                        break;
                }

                throw new BadRequestException(ErrorCode.PWD_NOT_VALID, html);
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

        public async Task<LoginResult> GetLoginResultAsync(long customerId, CancellationToken cancellationToken)
        {
            var customerReadRepository = _serviceProvider.GetRequiredService<ICustomerReadRepository>();
            var customer = await customerReadRepository.GetByIdAsync(customerId, cancellationToken: cancellationToken);

            return await GetLoginResultAsync(customer, cancellationToken);
        }

        public async Task<LoginResult> GetLoginResultAsync(BaseUser user, CancellationToken cancellationToken)
        {
            var roles = user is Customer ? new List<Role>() : (user as Employee).EmployeeRoles.Select(x => x.Role);
            var actions = user is Customer ? new List<ActionWithExcludeValue>() : await _actionReadRepository.GetActionsByEmployeeIdAsync(user.Id, cancellationToken);
            var permission = user is Customer ? "15" : GetPermission(user as Employee, actions);
            var payload = new GenTokenPayload { User = user, Permission = permission, Roles = roles };
            var accessToken = await GenerateAccessTokenAsync(payload, cancellationToken);

            _executionContext.UpdateContext(accessToken);

            var sc = await GetSystemConfigurationAsync(cancellationToken);
            var result = new LoginResult
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(sc.Session ?? AuthConfig.RefreshTokenTime),
                IsPasswordChangeRequired = user.IsPasswordChangeRequired
            };

            // Save refresh token
            var refreshToken = new RefreshToken
            {
                RefreshTokenValue = result.RefreshToken,
                CurrentAccessToken = result.AccessToken,
                ExpiryDate = DateTime.Now.AddSeconds(sc.Session ?? AuthConfig.RefreshTokenTime),
            };

            var requestContext = _executionContext.HttpContext.Request;
            var origin = requestContext.Headers[HeaderNames.Origin];
            var loginHistory = new LoginHistory
            {
                UserId = user.Id,
                Ip = AuthUtility.TryGetIP(requestContext),
                Timestamp = DateTime.Now,
                UA = requestContext.Headers[HeaderNames.UserAgent],
                Origin = !string.IsNullOrEmpty(origin) ? origin : ""
            };

            _authRepository.UnitOfWork.BeginTransaction();

            await _authRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
            await _authRepository.AddLoginHistoryAsync(loginHistory, cancellationToken);
            await _authRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<List<AppToken>> GetLiveAccessTokensOfUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            var cacheEntry = CacheManager.GetAccessTokenCacheEntry(userId);
            var tokens = await _redisCache.GetAsync<List<AppToken>>(cacheEntry.Key, cancellationToken) ?? new();

            return tokens.Where(x => x.Expires >= DateTime.Now).ToList();
        }

        private async Task StoreAccessTokensAsync(long userId, List<AppToken> tokens, CancellationToken cancellationToken = default)
        {
            var cacheEntry = CacheManager.GetAccessTokenCacheEntry(userId);
            tokens = tokens.Where(x => x.Expires > DateTime.Now).ToList();

            await _redisCache.SetAsync(cacheEntry.Key, tokens, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }

        private Task<SystemConfiguration> GetSystemConfigurationAsync(CancellationToken cancellationToken = default)
        {
            var systemConfigurationRepository = _serviceProvider.GetRequiredService<ISystemConfigurationReadRepository>();
            return systemConfigurationRepository.GetAsync(cancellationToken);
        }

        public async Task ValidateAccessAndThrowAsync(BaseUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new BadRequestException(_localizer["Account.NotFound"]);
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new BadRequestException(_localizer["Account.IsInactive"]);
            }

            if (user.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException(_localizer["Account.IsBanned"]);
            }

            await Task.Yield();
            //var sc = await GetSystemConfigurationAsync(cancellationToken);
            //if (sc.IsEnabledVerifiedAccount == true && user.Status == AccountStatus.UnConfirm)
            //{
            //    throw new BadRequestException(_localizer["Authentication.AccountHasNotBeenVerified"]);
            //}
        }

        public void ValidateStateAndThrow(BaseUser user)
        {
            if (user == null)
            {
                throw new BadRequestException(_localizer["Account.NotFound"]);
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new BadRequestException(_localizer["Account.IsInactive"]);
            }

            if (user.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException(_localizer["Account.IsBanned"]);
            }

        }
    }
}
