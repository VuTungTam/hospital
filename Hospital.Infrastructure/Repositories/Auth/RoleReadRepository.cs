using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;
namespace Hospital.Infrastructure.Repositories.Auth
{
    public class RoleReadRepository : ReadRepository<Role>, IRoleReadRepository
    {
        public RoleReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<List<Action>> GetCustomerActions(CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetCustomerPermission();

            var valueFactory = () =>  _dbContext.Roles
                .Where(r => r.Code == RoleCodeConstant.CUSTOMER)
                .SelectMany(r => r.RoleActions.Select(ra => ra.Action))
                .ToListAsync(cancellationToken);

            var actions = await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);

            return actions;
        }

        public override async Task<PaginationResult<Role>> GetPaginationAsync(Pagination pagination, ISpecification<Role> spec, QueryOption option, CancellationToken cancellationToken = default)
        {
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
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

            return new PaginationResult<Role>(data, count);
        }
    }
}
