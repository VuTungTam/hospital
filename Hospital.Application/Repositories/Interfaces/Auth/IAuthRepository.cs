using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;

namespace Hospital.Application.Repositories.Interfaces.Auth
{
    public interface IAuthRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task AddLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken);

        Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken);

        Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

        void UpdateRefreshToken(RefreshToken refreshToken);

        Task RemoveRefreshTokenAsync(string currentAccessToken, CancellationToken cancellationToken);

        Task RemoveRefreshTokensAsync(IEnumerable<long> userIds, CancellationToken cancellationToken);
    }
}
