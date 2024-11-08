using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceReadRepository : ReadRepository<HealthService>, IHealthServiceReadRepository
    {
        public HealthServiceReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PagingResult<HealthService>> GetPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long serviceTypeId = 0, long facilityId = 0, long specialtyId = 0, bool ignoreOwner = false, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthService> spec = new GetHealthServicesByStatusSpecification(status);
            if (serviceTypeId > 0)
            {
                spec = spec.And(new GetHealthServicesByTypeSpecification(serviceTypeId));
            }

            if (facilityId > 0)
            {
                spec = spec.And(new GetHealthServicesByFacilityIdSpecification(facilityId));
            }

            if (specialtyId > 0)
            {
                spec = spec.And(new GetHealthServicesBySpecialtyIdSpecification(specialtyId));
            }

            var guardExpression = GuardDataAccess(spec, ignoreOwner).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Modified ?? x.Created);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<HealthService>(data, count);
        }
    }
}
