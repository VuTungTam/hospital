using Hospital.Domain.Enums;

namespace Hospital.Application.Dtos.Bookings
{
    public class BookingRequestDto : BaseDto
    {
        public string Code { get; set; }

        public string HealthProfileId { get; set; }

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public long TimeSlotId { get; set; }

        public string OwnerId { get; set; }
    }
}
