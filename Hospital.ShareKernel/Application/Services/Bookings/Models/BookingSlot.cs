namespace Hospital.SharedKernel.Application.Services.Bookings.Models
{
    public class BookingSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Order { get; set; }
    }

}


