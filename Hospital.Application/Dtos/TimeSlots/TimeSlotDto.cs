using Hospital.Domain.Entities.ServiceTimeRules;

namespace Hospital.Application.Dtos.TimeSlots
{
    public class TimeSlotDto : BaseDto
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public long TimeRuleId { get; set; }

        public ServiceTimeRule ServiceTimeRule { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
