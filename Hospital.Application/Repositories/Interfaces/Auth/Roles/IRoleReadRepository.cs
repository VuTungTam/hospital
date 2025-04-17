using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Models.Auths;
using System.Threading;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;

namespace Hospital.Application.Repositories.Interfaces.Auth.Roles
{
    public interface IRoleReadRepository : IReadRepository<Role>
    {
        Task<List<Action>> GetCustomerActions(CancellationToken cancellationToken);
    }
}
