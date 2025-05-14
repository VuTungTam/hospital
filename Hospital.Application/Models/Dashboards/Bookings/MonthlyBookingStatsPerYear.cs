namespace Hospital.Application.Models.Dashboards.Bookings
{
    public class MonthlyBookingStatsPerYear
    {
        public List<MonthlyBookingStatsValue> MonthValues { get; set; } = new();
    }

    public class MonthlyBookingStatsValue
    {
        public int Month { get; set; }

        public string MonthText => "T" + Month;

        public int TotalCount { get; set; }

        public int CompletedCount { get; set; }

        public int OtherCount => TotalCount - CompletedCount;
    }
}
