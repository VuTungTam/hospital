using Hospital.Application.Models;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Modules.Notifications.Entities;

namespace Hospital.Application.Repositories.Interfaces.Employees
{
    public interface IEmployeeWriteRepository : IWriteRepository<Employee>
    {
        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

        Task AddFacilityAdminAsync(Employee employee, long facilityId, CancellationToken cancellationToken);

        Task AddNotificationForEmployeeAsync(Notification notification, long facilityId, long zoneId, CallbackWrapper callbackWrapper, CancellationToken cancellationToken);

        Task UpdateRolesAsync(long employeeId, IEnumerable<long> roleIds, CancellationToken cancellationToken);

        //Task UpdateBranchesAsync(long employeeId, IEnumerable<long> branchIds, CancellationToken cancellationToken);

        Task UpdateStatusAsync(long employeeId, AccountStatus status, CancellationToken cancellationToken);

        Task UpdateLastSeenAsync(CancellationToken cancellationToken = default);
    }
}
