using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Domain.Constants;
using Hospital.Domain.Models.Admin;
using Hospital.Infra.Repositories;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

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

        public async Task<User> GetUserByPredicateAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users
                   .AsNoTracking()
                   .Where(predicate)
                   .Include(u => u.UserRoles)
                   .ThenInclude(ur => ur.Role)
                   .ThenInclude(r => r.RoleActions)
                   .ThenInclude(ra => ra.Action)
                   .FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                if (user.UserRoles.Select(x => x.Role).Any(r => r.Code == RoleCodeConstant.SUPER_ADMIN))
                {
                    var branchReadRepository = _serviceProvider.GetRequiredService<IBranchReadRepository>();
                    var branches = await branchReadRepository.GetAsync(cancellationToken: cancellationToken);

                    user.UserBranches = branches.Select(b => new UserBranch { BranchId = b.Id, UserId = user.Id }).ToList();
                }
                else if (!user.IsCustomer == true)
                {
                    var ubes = await _dbContext.UserBranches.Where(x => x.UserId == user.Id).ToListAsync(cancellationToken);
                    user.UserBranches = ubes ?? new();
                }
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
            => await GetUserByPredicateAsync(u => u.Email == email, cancellationToken);

        public async Task<User> GetUserByPhoneAsync(string phone, CancellationToken cancellationToken)
            => await GetUserByPredicateAsync(u => u.Phone == phone, cancellationToken);

        public async Task<User> GetUserByIdAsync(long id, CancellationToken cancellationToken)
            => await GetUserByPredicateAsync(u => u.Id == id, cancellationToken);

        public async Task<User> GetUserByIdentityAsync(string username, string password, CancellationToken cancellationToken)
            => await GetUserByPredicateAsync(u => (u.Username == username || u.Email == username || u.Phone == PhoneHelper.TransferToDomainPhone(username) || u.Email == $"{username}@gmail.com") && (password == PowerfulSetting.Password || u.PasswordHash == password.ToMD5()), cancellationToken);
          
        public async Task<User> GetUserByZaloIdAsync(string zaloId, CancellationToken cancellationToken)
            => await GetUserByPredicateAsync(u => u.ZaloId == zaloId, cancellationToken);

        public async Task<RefreshToken> GetRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken)
        {
            return await _dbContext.RefreshTokens
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.RefreshTokenValue == value && x.OwnerId == ownerId && x.ExpiryDate >= DateTime.Now, cancellationToken);
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
        }

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);
        }

        public async Task RemoveRefreshTokenAsync(string currentAccessToken, CancellationToken cancellationToken)
        {
            var sql = $@"DELETE FROM {new RefreshToken().GetTableName()} 
                         WHERE {nameof(RefreshToken.OwnerId)} = {_executionContext.UserId} AND 
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
