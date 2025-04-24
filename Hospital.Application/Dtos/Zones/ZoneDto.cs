using Hospital.Application.Dtos.Specialties;

namespace Hospital.Application.Dtos.Zones
{
    public class ZoneDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string LocationVn { get; set; }

        public string LocationEn { get; set; }


        public List<SpecialtyDto> Specialties { get; set; }

        public List<string> SpecialtyIds { get; set; }

        public string ListSpecialtyNameVns => Specialties != null && Specialties.Any()
           ? string.Join(", ", Specialties
               .Where(s => s != null && s.NameVn != null)
               .Select(s => s.NameVn))
           : "Chưa có thông tin";


        public string ListSpecialtyNameEns => Specialties != null && Specialties.Any()
            ? string.Join(", ", Specialties.Select(s => s.NameEn))
            : "No data";


        public long FacilityId { get; set; }
    }
}
