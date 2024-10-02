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
        private readonly string _traceId;
        private readonly IHttpContextAccessor _accessor;
        private readonly HttpContext _httpContext;
        private string _accessToken;
        private string _username;
        private string _permission;
        private string _uid;
        private long _branchId;
        private List<long> _branchIds;
        private long _userId;
        //private int _shard;
        private (bool HasValue, bool Value) _superAdminValue;
        public AccountType _accountType;

        public ExecutionContext(IHttpContextAccessor accessor)
        {
            _traceId = Guid.NewGuid().ToString();
            _accessor = accessor;
            _httpContext = accessor.HttpContext;
            _uid = _accessor.HttpContext?.Request.Headers[HeaderNamesExtension.Uid];
            _accessToken = GetAccessToken();

            if (!string.IsNullOrEmpty(_accessToken))
            {
                SetInformation(_accessToken);
            }
        }

        public string TraceId => _traceId;

        public string AccessToken => _accessToken;

        public string Username => _username;

        public string Permission => _permission;

        public string Uid => _uid;

        public AccountType AccountType => _accountType;

        public long BranchId => _branchId;

        public List<long> BranchIds => _branchIds;

        public long UserId => _userId;

        //public int Shard => _shard;

        public bool IsAnonymous => string.IsNullOrEmpty(AccessToken);

        public HttpContext HttpContext => _httpContext;

        public void UpdateContext(string accessToken)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                SetInformation(accessToken);
            }
        }

        public void MakeAnonymousRequest()
        {
            _accessToken = string.Empty;
            _userId = default;
            _username = default;
            _permission = default;
        }

        public bool IsSuperAdmin()
        {
            if (!_superAdminValue.HasValue)
            {
                var action = AuthUtility.FromExponentToPermission((int)ActionExponent.Master);
                _superAdminValue.Value = AuthUtility.ComparePermissionAsString(_permission, action);
                _superAdminValue.HasValue = true;
            }
            return _superAdminValue.Value;
        }

        private string GetAccessToken()
        {
            var bearerToken = _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString();
            if (string.IsNullOrEmpty(bearerToken) || bearerToken.Equals("Bearer"))
            {
                return "";
            }
            return bearerToken[7..];
        }

        private void SetInformation(string accessToken)
        {
            try
            {
                _accessToken = accessToken;
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(_accessToken);
                var claims = jwtSecurityToken.Claims;

                _branchId = Convert.ToInt64(claims.First(c => c.Type == ClaimConstant.BRANCH_ID).Value);
                _branchIds = JsonConvert.DeserializeObject<List<long>>(claims.First(c => c.Type == ClaimConstant.BRANCH_IDS).Value);
                _userId = Convert.ToInt64(claims.First(c => c.Type == ClaimConstant.USER_ID).Value);
                //_shard = Convert.ToInt32(claims.First(c => c.Type == ClaimConstant.SHARDING).Value);
                _username = claims.First(c => c.Type == ClaimConstant.USERNAME).Value;
                _permission = claims.First(c => c.Type == ClaimConstant.PERMISSION).Value;
                _accountType = (AccountType)Convert.ToInt32(claims.First(c => c.Type == ClaimConstant.ACCOUNT_TYPE).Value);
                _uid = _accessor.HttpContext?.Request.Headers[HeaderNamesExtension.Uid];
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Failed when validate access token {Exception}", ex);
                throw new UnauthorizeException();
            }
        }
    }
}
