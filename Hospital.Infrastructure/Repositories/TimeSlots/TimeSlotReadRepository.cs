using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.TimeSlots
{
    public class TimeSlotReadRepository : ReadRepository<TimeSlot>, ITimeSlotReadRepository
    {
        public TimeSlotReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
        public async Task<List<TimeSlot>> GetByTimeRuleIdAsync(long timeRuleId, CancellationToken cancellationToken)
        {
            CacheEntry cacheEntry = CacheManager.GetTimeSlotsEntry(timeRuleId);
            var data = await _redisCache.GetAsync<List<TimeSlot>>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                data = await _dbSet.AsNoTracking().Where(x => x.TimeRuleId == timeRuleId).ToListAsync(cancellationToken);
                if (data != null && data.Any())
                {
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken);
                }
            }
            return data;
        }

    }

}
