using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Auths;

namespace Hospital.Application.Repositories.Interfaces.Auth.Roles
{
    public interface IRoleWriteRepository : IWriteRepository<Role>
    {
        Task RemoveRoleActionAsync(RoleAction roleAction, CancellationToken cancellationToken);
    }
}
