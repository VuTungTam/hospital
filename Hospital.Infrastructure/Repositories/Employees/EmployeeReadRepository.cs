using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Employees
{
    public class EmployeeReadRepository : ReadRepository<Employee>, IEmployeeReadRepository
    {
        public EmployeeReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public Task<List<Employee>> GetAssigneesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByIdIncludedBranchesAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByZaloIdlAsync(string zaloId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<Employee>> GetEmployeesPaginationResultAsync(Pagination pagination, long branchId = 0, AccountStatus status = AccountStatus.None, long roleId = 0, bool includeBranches = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> GetSuperAdminsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmployeeCustomizePermissionAsync(long employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
