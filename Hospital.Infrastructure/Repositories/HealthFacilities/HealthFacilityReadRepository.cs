using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthFacilities;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Hospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityReadRepository : ReadRepository<HealthFacility>, IHealthFacilityReadRepository
    {
        public HealthFacilityReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PaginationResult<HealthFacility>> GetPagingWithFilterAsync(Pagination pagination, long facilityTypeId = 0, HealthFacilityStatus status = default, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthFacility> spec = new GetHealthFacilitiesByStatusSpecification(status);
            if (facilityTypeId > 0)
            {
                spec = spec.And(new GetHealthFacilitiesByTypeIdSpecification(facilityTypeId));
            }
            QueryOption option = new QueryOption();
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthFacility>(data, count);
        }
    }
}
