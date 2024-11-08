using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.ServiceTimeRules
{
    public interface IServiceTimeRuleWriteRepository : IWriteRepository<ServiceTimeRule>
    {
    }
}
