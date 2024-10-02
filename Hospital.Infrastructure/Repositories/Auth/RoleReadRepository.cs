using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Auth
{
    public class RoleReadRepository : ReadRepository<Role>, IRoleReadRepository
    {
        public RoleReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override async Task<PagingResult<Role>> GetPagingAsync(Pagination pagination, ISpecification<Role> spec = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
        {
            var guardExpression = GuardDataAccess(spec, ignoreOwner, ignoreBranch).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Include(x => x.RoleActions)
                         .ThenInclude(x => x.Action)
                         .Where(guardExpression);
            //.OrderBy(x => x.Name);

            var data = await query.ToListAsync(cancellationToken);
            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PagingResult<Role>(data, count);
        }
    }
}
