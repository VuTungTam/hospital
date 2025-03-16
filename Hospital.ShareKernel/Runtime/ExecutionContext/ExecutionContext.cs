using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace Hospital.SharedKernel.Runtime.ExecutionContext
{
    public class ExecutionContext : IExecutionContext
    {
        public string TraceId { get; private set; }

        public string AccessToken { get; private set; }

        public string Email { get; private set; }

        public string Permission { get; private set; }

        public string Uid { get; private set; }

        public AccountType AccountType { get; private set; }

        public long Identity { get; private set; }

        public bool IsAnonymous => string.IsNullOrEmpty(AccessToken);

        public bool IsSA { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public ExecutionContext(IHttpContextAccessor accessor)
        {
            HttpContext = accessor.HttpContext;
            TraceId = Guid.NewGuid().ToString();
            Uid = HttpContext?.Request.Headers[HeaderNamesExtension.Uid];
            AccessToken = GetAccessToken();

            if (!string.IsNullOrEmpty(AccessToken))
            {
                PopulateInformation(AccessToken);
            }
        }

        public void UpdateContext(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return;
            }

            PopulateInformation(accessToken);
        }

        public void MakeAnonymousRequest()
        {
            AccessToken = string.Empty;
            Identity = default;
            Permission = default;
        }

        private string GetAccessToken()
        {
            var bearerToken = HttpContext?.Request.Headers[HeaderNames.Authorization].ToString();
            if (string.IsNullOrEmpty(bearerToken) || bearerToken.Equals("Bearer"))
            {
                return "";
            }
            return bearerToken[7..];
        }

        private void PopulateInformation(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(accessToken);
                var claims = jwtSecurityToken.Claims;
                var action = AuthUtility.FromExponentToPermission((int)ActionExponent.Master);

                AccessToken = accessToken;
                Identity = Convert.ToInt64(claims.First(c => c.Type == ClaimConstant.USER_ID).Value);
                Email = claims.First(c => c.Type == ClaimConstant.EMAIL).Value;
                Permission = claims.First(c => c.Type == ClaimConstant.PERMISSION).Value;
                AccountType = (AccountType)Convert.ToInt32(claims.First(c => c.Type == ClaimConstant.ACCOUNT_TYPE).Value);
                Uid = HttpContext?.Request.Headers[HeaderNamesExtension.Uid];
                IsSA = AuthUtility.ComparePermissionAsString(Permission, action);
            }
            catch (Exception ex)
            {
                throw new UnauthorizeException(ex.Message);
            }
        }
    }
}
