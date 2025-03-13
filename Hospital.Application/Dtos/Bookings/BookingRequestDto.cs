using Hospital.Domain.Enums;

namespace Hospital.Application.Dtos.Bookings
{
    public class BookingRequestDto : BaseDto
    {
        public string Code { get; set; }

        public string HealthProfileId { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public string OwnerId { get; set; }
    }
}
