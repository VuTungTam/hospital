using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Domain.Constants;
using Hospital.Domain.Models.Admin;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Employees;
using Hospital.Domain.Specifications.Specialties;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Employees
{
    public class EmployeeReadRepository : ReadRepository<Employee>, IEmployeeReadRepository
    {
        public EmployeeReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override ISpecification<Employee> GuardDataAccess<Employee>(ISpecification<Employee> spec, QueryOption option = default)
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<Employee>(x => true);

            spec = spec.And(base.GuardDataAccess(spec, option));

            if (_executionContext.IsSA)
            {
                var adminSpec = new GetFacilityAdminSpecification();
                var selfSpec = new IdEqualsSpecification<Employee>(_executionContext.Identity);

                var spec2 = selfSpec.Or((ISpecification<Employee>)adminSpec);

                return spec.And(spec2);
            }

            spec = spec.And(new LimitByFacilityIdSpecification<Employee>(_executionContext.FacilityId));

            if (!option.IgnoreZone)
            {
                if (_executionContext.ZoneId == 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Employee>(_executionContext.FacilityId));
                }
                else
                {
                    spec = spec.And(new LimitByZoneIdSpecification<Employee>(_executionContext.ZoneId));
                }
            }

            return spec;
        }

        public async Task<Employee> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default)
        {
            var spec = new EmployeeByEmailEqualsSpecification(email)
                   .Or(new EmployeeByEmailEqualsSpecification($"{email}@gmail.com"));

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

        public async Task<Employee> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> spec = new ExpressionSpecification<Employee>(x => true);

            if (id != _executionContext.Identity)
            {
                spec = GuardDataAccess(spec);
            }

            var employee = await _dbSet.AsNoTracking()
                                       .Where(spec.GetExpression())
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

        public async Task<PaginationResult<Employee>> GetEmployeesPaginationResultAsync(Pagination pagination, AccountStatus status = AccountStatus.None, long zoneId = 0, long roleId = 0, long facilityId = 0, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            var includable = query.Include(x => x.EmployeeRoles)
                                  .ThenInclude(x => x.Role);
            query = includable;

            ISpecification<Employee> spec = new ExpressionSpecification<Employee>(x => true);

            if (status != AccountStatus.None)
            {
                spec = spec.And(new EmployeeByStatusEqualsSpecification(status));
            }

            if (facilityId > 0)
            {
                spec = spec.And(new EmployeeByFacilityIdEqualsSpecification(facilityId));
            }

            var option = new QueryOption();

            if (zoneId == 0)
            {
                option.IgnoreZone = true;
            }

            spec = GuardDataAccess(spec, option);

            query = query.Where(spec.GetExpression());

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

    }
}
