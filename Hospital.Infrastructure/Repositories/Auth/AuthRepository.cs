using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Auths;
using Hospital.Infrastructure.Repositories;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories.Auth
{
    public class AuthRepository : OrmRepository, IAuthRepository
    {
        public AuthRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.Database.BeginTransaction();
            }
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task AddLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
            => await _dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);

        public Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken)
        {
            var spec = new RefreshTokenByValueEqualsSpecification(value)
                  .And(new OwnerIdEqualsSpecification<RefreshToken>(ownerId))
                  .And(new RefreshTokenByExpiryDateGreaterThanSpecification(DateTime.Now));

            return _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
            => await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);
        }

        public async Task RemoveRefreshTokenAsync(string currentAccessToken, CancellationToken cancellationToken)
        {
            var sql = $@"DELETE FROM {new RefreshToken().GetTableName()} 
                         WHERE {nameof(RefreshToken.OwnerId)} = {_executionContext.Identity} AND 
                               {nameof(RefreshToken.CurrentAccessToken)} = '{currentAccessToken}'
                        ";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }

        public async Task RemoveRefreshTokensAsync(IEnumerable<long> userIds, CancellationToken cancellationToken)
        {
            var sql = $@"DELETE FROM {new RefreshToken().GetTableName()} 
                         WHERE {nameof(RefreshToken.OwnerId)} IN ({string.Join(", ", userIds)})";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }
    }
}
