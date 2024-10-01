using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;

namespace Hospital.Application.Repositories.Interfaces.Auth
{
    public interface IAuthRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<User> GetUserByIdentityAsync(string username, string password, CancellationToken cancellationToken);

        Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken);

        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<User> GetUserByPhoneAsync(string phone, CancellationToken cancellationToken);

        Task<User> GetUserByZaloIdAsync(string zaloId, CancellationToken cancellationToken);

        Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken);

        void AddRefreshToken(RefreshToken refreshToken);

        void UpdateRefreshToken(RefreshToken refreshToken);

        Task RemoveRefreshTokenAsync(string currentAccessToken, CancellationToken cancellationToken);

        Task RemoveRefreshTokensAsync(IEnumerable<long> userIds, CancellationToken cancellationToken);
    }
}
