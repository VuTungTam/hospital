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
        public static List<string> DefaultRandomPassword = new List<string>
        {
            "XukaYeuChaien",
            "NobitaDiHonda",
            "NobitaChaXeko",
            "DekhiYeuMimi",
            "DoremonDiLonTon",
            "ChaienBeNho",
            "XukaMoNhon"
        };

        public static List<string> Colors = new List<string> { "#191970", "#2E8B57", "#000080", "#00008B", "#0000CD", "#006400", "#4169E1", "#228B22", "#2F4F4F", "#008080", "#808000", "#556B2F", "#8B4513", "#D2691E", "#B22222", "#800000", "#8B0000", "#DC143C", "#FF1493", "#FF69B4", "#C71585", "#8B008B", "#4B0082", "#800080", "#9932CC", "#8A2BE2", "#483D8B", "#008B8B", "#4682B4", "#1E90FF", "#3CB371", "#32CD32", "#00FF7F", "#00CED1", "#00BFFF", "#6A5ACD", "#9370DB", "#FF8C00", "#FF4500", "#FF6347", "#FF7F50", "#CD5C5C", "#CD853F", "#BC8F8F", "#A52A2A", "#BA55D3", "#A0522D", "#708090", "#696969", "#000000", "#9400D3", "#8B4513", "#6B8E23", "#2E8B57", "#BDB76B", "#B8860B", "#DAA520", "#E9967A", "#F08080", "#FA8072", "#FFA07A", "#9932CC", "#7B68EE", "#00008B", "#00CED1", "#8FBC8F", "#20B2AA", "#66CDAA", "#483D8B", "#008080", "#191970", "#B22222", "#BC8F8F", "#8B4513", "#DAA520", "#556B2F", "#CD853F", "#E9967A", "#F08080", "#7B68EE", "#4169E1", "#708090", "#9932CC", "#778899", "#008B8B", "#228B22", "#7CFC00", "#32CD32", "#4682B4", "#2E8B57", "#C71585", "#FF6347", "#FF4500", "#FF7F50", "#D2691E", "#CD5C5C", "#FF8C00", "#8B0000", "#DC143C" };

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
                employee.Password = DefaultRandomPassword[random.Next(0, DefaultRandomPassword.Count)];
                employee.IsDefaultPassword = true;
                employee.IsPasswordChangeRequired = true;
            }

            var count = await _dbSet.IgnoreQueryFilters().CountAsync(cancellationToken);

            employee.Code = code.ValueString;
            employee.HashPassword();
            employee.ScheduleColor = Colors[count % Colors.Count];
            employee.FacilityId = _executionContext.FacilityId;
            employee.LastSeen = null;

            await _dbSet.AddAsync(employee, cancellationToken);
        }

        public async Task AddFacilityAdminAsync(Employee employee, long facilityId, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(employee.Password))
            {
                var random = new Random();
                employee.Password = DefaultRandomPassword[random.Next(0, DefaultRandomPassword.Count)];
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

            if (zoneId != 0)
            {
                query.Where(x => x.ZoneId == zoneId);
            }
            else
            {
                query.Where(x => x.FacilityId == facilityId);
            }

            var employeeReadRepository = _serviceProvider.GetRequiredService<IEmployeeReadRepository>();
            var notificationWriteRepository = _serviceProvider.GetRequiredService<INotificationWriteRepository>();

            var employees = await query.ToListAsync(cancellationToken);

            var removeCacheTasks = new List<Task>();
            foreach (var employee in employees)
            {
                var noti = JsonConvert.DeserializeObject<Notification>(JsonConvert.SerializeObject(notification));

                noti.OwnerId = employee.Id;
                notificationWriteRepository.Add(noti);

                removeCacheTasks.Add(notificationWriteRepository.RemovePaginationCacheByUserIdAsync(employee.Id, cancellationToken));
            }

            callbackWrapper.Callback = () => Task.WhenAll(removeCacheTasks);
        }

        public async Task SetActionAsDefaultAsync(long employeeId, CancellationToken cancellationToken)
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {new EmployeeAction().GetTableName()} WHERE {nameof(EmployeeAction.EmployeeId)} = {employeeId}");
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
        }

        public async Task SetAdditionalActionsAsync(long employeeId, List<AdditionalAction> actions, CancellationToken cancellationToken)
        {
            var table = new EmployeeAction().GetTableName();
            var values = string.Join(", ", actions.Select(a => $"({employeeId},{a.ActionId},{(a.IsExclude ? "b'1'" : "b'0'")})"));
            var deleteSql = $"DELETE FROM {table} WHERE {nameof(EmployeeAction.EmployeeId)} = {employeeId}";
            var insertSql = $"INSERT INTO {table} ({nameof(EmployeeAction.EmployeeId)}, ActionId, IsExclude) VALUES {values}";

            await _dbContext.Database.ExecuteSqlRawAsync(deleteSql + "; " + insertSql);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
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
