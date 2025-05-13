using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Models.Auths;

namespace Hospital.SharedKernel.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        string GetPermission(List<ActionWithExcludeValue> actions);

        Task<string> GetCustomerPermission(CancellationToken cancellationToken);

        Task<string> GetDoctorPermission(CancellationToken cancellationToken);

        bool CheckPermission(ActionExponent exponent);

        bool CheckPermission(ActionExponent[] exponents);

        bool CheckHasAnyPermission(ActionExponent[] exponents);

        Task CheckPasswordLevelAndThrowAsync(string pwd, CancellationToken cancellationToken);

        string GenerateRefreshToken(int exprie);

        Task<string> GenerateAccessTokenAsync(GenTokenPayload payload, CancellationToken cancellationToken = default);

        Task RevokeAccessTokenAsync(long userId, string accessToken, CancellationToken cancellationToken);

        Task<LoginResult> GetLoginResultAsync(long customerId, CancellationToken cancellationToken);

        Task<LoginResult> GetLoginResultAsync(BaseUser user, CancellationToken cancellationToken);

        Task<List<AppToken>> GetLiveAccessTokensOfUserAsync(long userId, CancellationToken cancellationToken = default);

        Task ForceLogoutAsync(long userId, CancellationToken cancellationToken);

        Task FetchNewTokenAsync(long userId, string message, CancellationToken cancellationToken);

        Task ValidateAccessAndThrowAsync(BaseUser user, CancellationToken cancellationToken);

        void ValidateStateAndThrow(BaseUser user);
    }
}
