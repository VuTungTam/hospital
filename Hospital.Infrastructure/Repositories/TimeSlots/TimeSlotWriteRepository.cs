using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.TimeSlots
{
    public class TimeSlotWriteRepository : WriteRepository<TimeSlot>, ITimeSlotWriteRepository
    {
        public TimeSlotWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }
        
    }
}
