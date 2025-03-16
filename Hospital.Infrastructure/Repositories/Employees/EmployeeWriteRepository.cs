using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Employees
{
    public class EmployeeWriteRepository : WriteRepository<Employee>, IEmployeeWriteRepository
    {
        public EmployeeWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetActionAsDefaultAsync(long employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBranchesAsync(long employeeId, IEnumerable<long> branchIds, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLastSeenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRolesAsync(long employeeId, IEnumerable<long> roleIds, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(Employee employee, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
