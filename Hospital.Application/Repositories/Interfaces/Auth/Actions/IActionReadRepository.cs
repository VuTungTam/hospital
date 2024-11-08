using Hospital.SharedKernel.Application.Repositories.Interface;
using Action = Hospital.SharedKernel.Application.Services.Auth.Entities.Action;
namespace Hospital.Application.Repositories.Interfaces.Auth.Actions
{
    public interface IActionReadRepository : IReadRepository<Action>
    {
        Task<Action> GetMasterAsync(CancellationToken cancellationToken);
    }
}
