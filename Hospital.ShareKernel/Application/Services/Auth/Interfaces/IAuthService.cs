using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        string GetPermission(User user);

        bool CheckPermission(ActionExponent exponent);

        bool CheckPermission(ActionExponent[] exponents);

        bool CheckHasAnyPermission(ActionExponent[] exponents);

        Task CheckPasswordLevelAndThrowAsync(string pwd, CancellationToken cancellationToken);

        string GenerateRefreshToken();

        Task<string> GenerateAccessTokenAsync(User user, string permission, IEnumerable<Role> roles, long selectedBranchId = default, CancellationToken cancellationToken = default);

        Task RevokeAccessTokenAsync(long userId, string accessToken, CancellationToken cancellationToken);

        Task<LoginResult> GetLoginResultAsync(long userId, CancellationToken cancellationToken);

        Task<LoginResult> GetLoginResultAsync(User user, CancellationToken cancellationToken);

        Task<List<AppToken>> GetLiveAccessTokensOfUserAsync(long userId, CancellationToken cancellationToken = default);

        Task ForceLogoutAsync(long userId, CancellationToken cancellationToken);

        Task ValidateAccessAndThrowAsync(User user, CancellationToken cancellationToken);

        void ValidateStateAndThrow(User user);

        void ValidateStateIncludeActiveAndThrow(User user);
    }
}
