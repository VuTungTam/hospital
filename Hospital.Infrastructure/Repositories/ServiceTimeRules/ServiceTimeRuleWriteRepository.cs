using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.Extensions.Localization;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hospital.Infrastructure.Repositories.ServiceTimeRules
{
    public class ServiceTimeRuleWriteRepository : WriteRepository<ServiceTimeRule>, IServiceTimeRuleWriteRepository
    {
        public ServiceTimeRuleWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {

        }
        public List<TimeSlot> GenerateTimeSlots(ServiceTimeRule timeRule)
        {
            TimeSpan start = timeRule.StartTime;
            TimeSpan end = timeRule.EndTime;
            TimeSpan startBreak = timeRule.StartBreakTime;
            TimeSpan endBreak = timeRule.EndBreakTime;
            int duration = timeRule.SlotDuration;
            bool allowWalkin = timeRule.AllowWalkin;

            var timeSlots = new List<TimeSlot>();

            var morningSlots = new List<TimeSlot>();
            TimeSpan begin = start;
            while (begin.Add(TimeSpan.FromMinutes(duration)) <= startBreak)
            {
                morningSlots.Add(new TimeSlot
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    Start = begin,
                    End = begin.Add(TimeSpan.FromMinutes(duration)),
                    TimeRuleId = timeRule.Id,
                });
                begin = begin.Add(TimeSpan.FromMinutes(duration));
            }
            if (morningSlots.Any() && allowWalkin)
            {
                morningSlots.Last().ForWalkin = true;
            }
            timeSlots.AddRange(morningSlots);

            var afternoonSlots = new List<TimeSlot>();
            TimeSpan begin2 = endBreak;
            while (begin2.Add(TimeSpan.FromMinutes(duration)) <= end)
            {
                afternoonSlots.Add(new TimeSlot
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    Start = begin2,
                    End = begin2.Add(TimeSpan.FromMinutes(duration)),
                    TimeRuleId = timeRule.Id,
                });
                begin2 = begin2.Add(TimeSpan.FromMinutes(duration));
            }
            if (afternoonSlots.Any() && allowWalkin)
            {
                afternoonSlots.Last().ForWalkin = true;
            }
            timeSlots.AddRange(afternoonSlots);

            return timeSlots;
        }

    }
}
