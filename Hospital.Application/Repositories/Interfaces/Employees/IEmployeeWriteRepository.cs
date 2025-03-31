using Hospital.Application.Models;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Employees
{
    public interface IEmployeeWriteRepository : IWriteRepository<Employee>
    {
        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

        //Task AddBookingNotificationForEmployeesAsync(Notification notification, long branchId, CallbackWrapper callbackWrapper, CancellationToken cancellationToken);

        Task SetActionAsDefaultAsync(long employeeId, CancellationToken cancellationToken);

        Task SetAdditionalActionsAsync(long employeeId, List<AdditionalAction> actions, CancellationToken cancellationToken);

        Task UpdateRolesAsync(long employeeId, IEnumerable<long> roleIds, CancellationToken cancellationToken);

        //Task UpdateBranchesAsync(long employeeId, IEnumerable<long> branchIds, CancellationToken cancellationToken);

        Task UpdateStatusAsync(long employeeId, AccountStatus status, CancellationToken cancellationToken);

        Task UpdateLastSeenAsync(CancellationToken cancellationToken = default);
    }
}
