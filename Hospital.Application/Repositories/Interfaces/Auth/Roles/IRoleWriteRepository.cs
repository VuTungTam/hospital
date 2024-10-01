using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Auth.Entities;

namespace Hospital.Application.Repositories.Interfaces.Auth.Roles
{
    public interface IRoleWriteRepository : IWriteRepository<Role>
    {
        Task RemoveRoleActionAsync(RoleAction roleAction, CancellationToken cancellationToken);
    }
}
