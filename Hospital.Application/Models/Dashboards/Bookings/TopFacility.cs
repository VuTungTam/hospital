namespace Hospital.Application.Models.Dashboards.Bookings
{
    public class TopFacility
    {
        public List<TopFacilityValue> Values { get; set; } = new();
    }

    public class TopFacilityValue
    {
        public string FacilityNameVn { get; set; }

        public string FacilityNameEn { get; set; }

        public long FacilityId { get; set; }

        public int TotalCount { get; set; }

    }
}
