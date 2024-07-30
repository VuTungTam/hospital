using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Enums;

namespace Hospital.Application.Dtos.Locations
{
    public class DistrictDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProvinceId { get; set; }

        public LocationType Type { get; set; }
    }
}
