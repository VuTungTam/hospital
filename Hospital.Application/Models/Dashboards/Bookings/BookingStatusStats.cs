namespace Hospital.Application.Models.Dashboards.Bookings
{
    public class BookingStatusStats
    {
        public List<BookingStatusStatsValue> Values { get; set; } = new();
    }

    public class BookingStatusStatsValue
    {
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int TotalCount { get; set; }

    }
}
