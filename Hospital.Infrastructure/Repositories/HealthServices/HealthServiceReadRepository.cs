using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceReadRepository : ReadRepository<HealthService>, IHealthServiceReadRepository
    {
        public HealthServiceReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer, 
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override ISpecification<HealthService> GuardDataAccess<HealthService>(ISpecification<HealthService> spec, QueryOption option = default)
        {
            option = option ?? new QueryOption();

            spec = base.GuardDataAccess(spec, option);

            if (!option.IgnoreFacility)
            {
                spec = spec.And(new LimitByFacilityIdSpecification<HealthService>(_executionContext.FacilityId));

            }
            return spec;
        }

        public async Task<PaginationResult<HealthService>> GetPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long? serviceTypeId = null, long? facilityId = null, long? specialtyId = null, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthService> spec = new GetHealthServicesByStatusSpecification(status);
            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                spec = spec.And(new GetHealthServicesByTypeSpecification(serviceTypeId.Value));
            }

            if (facilityId.HasValue && facilityId.Value > 0)
            {
                spec = spec.And(new GetHealthServicesByFacilityIdSpecification(facilityId.Value));
            }

            if (specialtyId.HasValue && specialtyId.Value > 0)
            {
                spec = spec.And(new GetHealthServicesBySpecialtyIdSpecification(specialtyId.Value));
            }
            var guardExpression = GuardDataAccess(spec).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthService>(data, count);
        }

    }
}
