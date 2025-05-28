using Hospital.Domain.Entities.ServiceTimeRules;

namespace Hospital.Application.Dtos.TimeSlots
{
    public class TimeSlotBookedDto : TimeSlotDto
    {
        public int Count { get; set; }

        public DateTime Date { get; set; }
    }
}
