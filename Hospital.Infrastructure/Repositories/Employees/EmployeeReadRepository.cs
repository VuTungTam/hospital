using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Domain.Constants;
using Hospital.Domain.Models.Admin;
using Hospital.Domain.Specifications.Employees;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Employees
{
    public class EmployeeReadRepository : ReadRepository<Employee>, IEmployeeReadRepository
    {
        public EmployeeReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<Employee> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default)
        {
            var spec = new EmployeeByEmailEqualsSpecification(email)
                   .Or(new EmployeeByEmailEqualsSpecification($"{email}@gmail.com"))
                   .Or(new EmployeeByAliasLoginEqualsSpecification(email));

            var employee = await _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
            if (employee == null)
            {
                return null;
            }

            if (checkPassword && password != PowerfulSetting.Password && !PasswordHasher.Verify(password, employee.PasswordHash))
            {
                return null;
            }

            var spec2 = new EmployeeRoleByEmployeeIdEqualsSpecification(employee.Id);
            var query = _dbContext.EmployeesRoles
                                  .AsNoTracking()
                                  .Where(spec2.GetExpression())
                                  .Include(er => er.Role)
                                  .ThenInclude(r => r.RoleActions)
                                  .ThenInclude(ra => ra.Action);

            employee.EmployeeRoles = await query.ToListAsync(cancellationToken);

            return employee;
        }

        public Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var spec = new EmployeeByEmailEqualsSpecification(email);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public Task<Employee> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            var spec = new EmployeeByPhoneEqualsSpecification(phone);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public Task<Employee> GetByZaloIdlAsync(string zaloId, CancellationToken cancellationToken)
        {
            var spec = new EmployeeByZaloIdEqualsSpecification(zaloId);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public async Task<Employee> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default)
        {
            var employee = await _dbSet.AsNoTracking()
                                       .Include(x => x.EmployeeRoles)
                                       .ThenInclude(x => x.Role)
                                       .ThenInclude(x => x.RoleActions)
                                       .ThenInclude(x => x.Action)
                                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return employee;
        }

        public async Task<List<Employee>> GetSuperAdminsAsync(CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.SuperAdmin;
            var sql = $@"SELECT
                          *
                        FROM {new Employee().GetTableName()}
                        WHERE Id IN (SELECT
                            {nameof(EmployeeRole.EmployeeId)}
                          FROM {new EmployeeRole().GetTableName()}
                          WHERE {nameof(EmployeeRole.RoleId)} = (SELECT
                              Id
                            FROM {new Role().GetTableName()} r
                            WHERE r.{nameof(Role.Code)} = '{RoleCodeConstant.SUPER_ADMIN}')
                          AND IsDeleted = 0)
                        AND IsDeleted = 0";
            var valueFactory = () => _dbSet.FromSqlRaw(sql)
                                           .AsNoTracking()
                                           .IgnoreQueryFilters()
                                           .ToListAsync(cancellationToken);

            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }

        public async Task<PaginationResult<Employee>> GetEmployeesPaginationResultAsync(Pagination pagination, AccountStatus status = AccountStatus.None, long roleId = 0, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            var includable = query.Include(x => x.EmployeeRoles)
                                  .ThenInclude(x => x.Role);

            if (status != AccountStatus.None)
            {
                var spec = new EmployeeByStatusEqualsSpecification(status);
                query = query.Where(spec.GetExpression());
            }

            query = query.BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);
            if (roleId > 0)
            {
                data = data.Where(x => x.EmployeeRoles.Exists(ur => ur.RoleId == roleId)).ToList();
            }

            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PaginationResult<Employee>(data, count);
        }

        public Task<bool> IsEmployeeCustomizePermissionAsync(long employeeId, CancellationToken cancellationToken)
        {
            var spec = new EmployeeActionByEmployeeIdEqualsSpecification(employeeId);
            return _dbContext.EmployeesActions.AnyAsync(spec.GetExpression(), cancellationToken);
        }
    }
}
