namespace Hospital.Application.Models.Dashboards.Bookings
{
    public class YearlyFacilityBookingStats
    {
        public List<YearlyFacilityBookingStatsValue> Values { get; set; } = new();
    }

    public class YearlyFacilityBookingStatsValue
    {
        public long FacilityId { get; set; }

        public string FacilityName { get; set; }

        public int TotalCount { get; set; }

        public int CompletedCount { get; set; }

        public int OtherCount => TotalCount - CompletedCount;
    }
}
