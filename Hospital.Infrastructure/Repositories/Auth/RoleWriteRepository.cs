using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Auth
{
    public class RoleWriteRepository : WriteRepository<Role>, IRoleWriteRepository
    {
        public RoleWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task RemoveRoleActionAsync(RoleAction roleAction, CancellationToken cancellationToken)
        {
            _dbContext.RoleActions.Remove(roleAction);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
