using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Specifications.HealthProfiles;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.HealthProfiles
{
    public class HealthProfileReadRepository : ReadRepository<HealthProfile>, IHealthProfileReadRepository
    {
        public HealthProfileReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PagingResult<HealthProfile>> GetPagingWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> spec = null;

            if (userId > 0) {

                spec = new GetHealthProfileByOwnerIdSpecification(userId);

            }
            QueryOption option = new QueryOption
            {
                IgnoreOwner = true,
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Modified ?? x.Created);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<HealthProfile>(data, count);
        }
    }
}
