namespace Hospital.Application.Models.Dashboards.Services
{
    public class YearlyServiceStats
    {
        public List<YearlyServiceStatsValue> Values { get; set; } = new();
    }

    public class YearlyServiceStatsValue
    {
        public long ServiceId { get; set; }

        public string ServiceName { get; set; }
        public string ServiceNameEn { get; set; }

        public int TotalCount { get; set; }

        public int CompletedCount { get; set; }

        public int OtherCount => TotalCount - CompletedCount;
    }
}
