using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Models.Auths;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;
namespace Hospital.Application.Repositories.Interfaces.Auth.Actions
{
    public interface IActionReadRepository : IReadRepository<Action>
    {
        Task<Action> GetMasterAsync(CancellationToken cancellationToken);
        Task<List<ActionWithExcludeValue>> GetActionsByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default);
    }
}
