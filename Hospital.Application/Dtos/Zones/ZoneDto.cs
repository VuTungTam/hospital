using Hospital.Application.Dtos.Specialties;

namespace Hospital.Application.Dtos.Zones
{
    public class ZoneDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public List<SpecialtyDto> Specialties { get; set; }

        public long FacilityId { get; set; }
    }
}
