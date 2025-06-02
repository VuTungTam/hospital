using Hospital.Application.Models;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Hospital.Infrastructure.Repositories.Employees
{
    public class EmployeeWriteRepository : WriteRepository<Employee>, IEmployeeWriteRepository
    {
        public EmployeeWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new Employee().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            if (string.IsNullOrEmpty(employee.Password))
            {
                var random = new Random();
                employee.Password = "employee@1";
                employee.IsDefaultPassword = true;
                employee.IsPasswordChangeRequired = true;
            }

            var count = await _dbSet.IgnoreQueryFilters().CountAsync(cancellationToken);

            employee.Code = code.ValueString;
            employee.HashPassword();
            employee.FacilityId = _executionContext.FacilityId;
            employee.LastSeen = null;

            await _dbSet.AddAsync(employee, cancellationToken);
        }

        public async Task AddFacilityAdminAsync(Employee employee, long facilityId, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(employee.Password))
            {
                var random = new Random();
                employee.Password = "admin@1";
                employee.IsDefaultPassword = true;
                employee.IsPasswordChangeRequired = true;
            }


            employee.HashPassword();
            employee.FacilityId = facilityId;
            employee.LastSeen = null;

            await _dbSet.AddAsync(employee, cancellationToken);
        }

        public async Task AddNotificationForEmployeeAsync(Notification notification, long zoneId, long facilityId, CallbackWrapper callbackWrapper, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking()
                .Include(x => x.EmployeeRoles)
                .Where(x => x.EmployeeRoles.Any(er => er.Role.Code == RoleCodeConstant.COORDINATOR));

            query = query.Where(x => x.ZoneId == zoneId && x.FacilityId == facilityId);

            var employeeReadRepository = _serviceProvider.GetRequiredService<IEmployeeReadRepository>();
            var notificationWriteRepository = _serviceProvider.GetRequiredService<INotificationWriteRepository>();

            var employees = await query.ToListAsync(cancellationToken);

            var removeCacheTasks = new List<Task>();
            foreach (var employee in employees)
            {
                var noti = JsonConvert.DeserializeObject<Notification>(JsonConvert.SerializeObject(notification));
                noti.Id = AuthUtility.GenerateSnowflakeId();
                noti.OwnerId = employee.Id;
                notificationWriteRepository.Add(noti);

                removeCacheTasks.Add(notificationWriteRepository.RemovePaginationCacheByUserIdAsync(employee.Id, cancellationToken));
            }

            callbackWrapper.Callback = () => Task.WhenAll(removeCacheTasks);
        }

        public async Task UpdateRolesAsync(long employeeId, IEnumerable<long> roleIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new EmployeeRole().GetTableName()} WHERE {nameof(EmployeeRole.EmployeeId)} = {employeeId}; ";
            foreach (var roleId in roleIds)
            {
                sql += $"INSERT INTO {new EmployeeRole().GetTableName()}(Id, RoleId, {nameof(EmployeeRole.EmployeeId)}, CreatedBy, CreatedAt) VALUES ({AuthUtility.GenerateSnowflakeId()},{roleId}, {employeeId}, {_executionContext.Identity}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }

        public async Task UpdateStatusAsync(long employeeId, AccountStatus status, CancellationToken cancellationToken)
        {
            var employee = new Employee { Id = employeeId, Status = status };
            _dbContext.Attach(employee);
            _dbContext.Entry(employee).Property(x => x.Status).IsModified = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateLastSeenAsync(CancellationToken cancellationToken = default)
        {
            var sql = $"UPDATE {new Employee().GetTableName()} SET LastSeen = GETDATE() WHERE Id = {_executionContext.Identity}; ";

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
