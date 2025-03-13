using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Constants;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.Users
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PagingResult<User>> GetEmployeesPagingResultAsync(Pagination pagination, AccountStatus state = AccountStatus.None, long roleId = 0, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Include(x => x.UserRoles)
                         .ThenInclude(x => x.Role)
                         .Where(x => x.IsCustomer == false);

            if (state != AccountStatus.None)
            {
                query = query.Where(x => x.Status == state);
            }
            query = query.BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);
            if (roleId > 0)
            {
                data = data.Where(x => x.UserRoles.Exists(ur => ur.RoleId == roleId)).ToList();
            }

            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PagingResult<User>(data, count);
        }

        public async Task<PagingResult<User>> GetCustomersPagingResultAsync(Pagination pagination, AccountStatus state = AccountStatus.None, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Where(x => x.IsCustomer == true);

            if (state != AccountStatus.None)
            {
                query = query.Where(x => x.Status == state);
            }
            var count = await query.CountAsync(cancellationToken);

            query = query.Skip(pagination.Offset)
                         .Take(pagination.Size)
                         .BuildOrderBy(pagination.Sorts);
            var data = await query.ToListAsync(cancellationToken);

            return new PagingResult<User>(data, count);
        }

        public async Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            if (exceptId <= 0)
            {
                return await _dbSet.AnyAsync(x => x.Phone == phone, cancellationToken);
            }
            return await _dbSet.AnyAsync(x => x.Phone == phone && x.Id != exceptId, cancellationToken);
        }

        public async Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            if (exceptId <= 0)
            {
                return await _dbSet.AnyAsync(x => x.Email == email, cancellationToken);
            }
            return await _dbSet.AnyAsync(x => x.Email == email && x.Id != exceptId, cancellationToken);
        }

        public async Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            if (exceptId <= 0)
            {
                return await _dbSet.AnyAsync(x => x.Code == code, cancellationToken);
            }
            return await _dbSet.AnyAsync(x => x.Code == code && x.Id != exceptId, cancellationToken);
        }

        public async Task<User> GetCurrentUserAsync(bool includeRole = false, CancellationToken cancellationToken = default)
        {
            if (!includeRole)
            {
                var executionContext = _serviceProvider.GetRequiredService<IExecutionContext>();
                return await base.GetByIdAsync(_executionContext.UserId, DefaultQueryOption, cancellationToken: cancellationToken);
            }
            return await _dbSet
                   .AsNoTracking()
                   .Include(x => x.UserRoles)
                   .ThenInclude(x => x.Role)
                   .FirstOrDefaultAsync(x => x.Id == _executionContext.UserId, cancellationToken: cancellationToken);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone, cancellationToken);
        }

        public async Task<User> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                   .AsNoTracking()
                   .Include(x => x.UserRoles)
                   .ThenInclude(x => x.Role)
                   .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetSuperAdminsAsync(CancellationToken cancellationToken)
        {
            var key = BaseCacheKeys.GetSuperAdminKey();
            var sql = $@"SELECT
                          *
                        FROM {new User().GetTableName()}
                        WHERE Id IN (SELECT
                            UserId
                          FROM {new UserRole().GetTableName()}
                          WHERE RoleId = (SELECT
                              Id
                            FROM {new Role().GetTableName()} r
                            WHERE r.Code = '{RoleCodeConstant.SUPER_ADMIN}')
                          AND Deleted IS NULL)
                        AND Deleted IS NULL";
            var valueFactory = () => _dbSet.FromSqlRaw(sql)
                                           .AsNoTracking()
                                           .IgnoreQueryFilters()
                                           .ToListAsync(cancellationToken);

            return await _redisCache.GetOrSetAsync(key, valueFactory, TimeSpan.FromSeconds(AppCacheTime.SuperAdmin), cancellationToken: cancellationToken);
        }

        public async Task<List<User>> GetCustomerNamesAsync(CancellationToken cancellationToken = default)
        {
            ISpecification<User> spec = null;
            QueryOption option = new QueryOption();
            var guard = GuardDataAccess(spec, option);
            var customers = await _dbSet.Where(guard.GetExpression())
                                  .Where(x => x.IsCustomer == true && (x.Status == AccountStatus.Active || x.Status == AccountStatus.UnConfirm))
                                  .OrderBy(x => x.Name)
                                  .ThenBy(x => x.Code)
                                  .Select(x => new User { Id = x.Id, Name = x.Name, Code = x.Code, Phone = x.Phone })
                                  .ToListAsync(cancellationToken);

            return customers;
        }
    }
}
