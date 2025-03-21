using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading;
using Hospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.SocialNetworks
{
    public class SocialNetworkReadRepository : ReadRepository<SocialNetwork>, ISocialNetworkReadRepository
    {
        public SocialNetworkReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PaginationResult<SocialNetwork>> GetPaging(Pagination pagination, CancellationToken cancellationToken)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<SocialNetwork>(data, count);
        }

    }
}
