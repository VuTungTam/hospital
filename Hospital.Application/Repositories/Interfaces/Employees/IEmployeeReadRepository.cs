using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Employees
{
    public interface IEmployeeReadRepository : IReadRepository<Employee>
    {
        Task<Employee> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default);

        Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<Employee> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

        Task<Employee> GetByZaloIdlAsync(string zaloId, CancellationToken cancellationToken);

        Task<List<Employee>> GetSuperAdminsAsync(CancellationToken cancellationToken);

        Task<List<Employee>> GetAssigneesAsync(CancellationToken cancellationToken);

        Task<Employee> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default);

        Task<Employee> GetByIdIncludedBranchesAsync(long id, CancellationToken cancellationToken = default);

        Task<PaginationResult<Employee>> GetEmployeesPaginationResultAsync(Pagination pagination, long branchId = 0, AccountStatus status = AccountStatus.None, long roleId = 0, bool includeBranches = false, CancellationToken cancellationToken = default);

        Task<bool> IsEmployeeCustomizePermissionAsync(long employeeId, CancellationToken cancellationToken);
    }
}
