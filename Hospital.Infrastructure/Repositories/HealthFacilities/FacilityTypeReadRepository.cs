using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Polly;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class FacilityTypeReadRepository : ReadRepository<FacilityType>, IFacilityTypeReadRepository
    {
        public FacilityTypeReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<List<(FacilityType Type, int Total)>> GetTypeAsync(CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetFacilityType();

            var valueFactory = () => _dbContext.FacilityTypeMappings
                .GroupBy(m => m.TypeId)
                .Select(g => new
                {
                    TypeId = g.Key,
                    Total = g.Count()
                })
                .Join(_dbContext.FacilityTypes,
                      g => g.TypeId,
                      t => t.Id,
                      (g, t) => new ValueTuple<FacilityType, int>(t, g.Total))
                .ToListAsync(cancellationToken);

            var facilityTypes = await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            
            return facilityTypes;
        }

    }
}
