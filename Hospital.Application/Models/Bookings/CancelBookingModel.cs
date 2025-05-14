namespace Hospital.Application.Models.Bookings
{
    public class CancelBookingModel
    {
        public long BookingId { get; set; }

        public string Reason { get; set; }

    }
}
