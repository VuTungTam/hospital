using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Enums;

namespace Hospital.Application.Dtos.Locations
{
    public class WardDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DistrictId { get; set; }

        public LocationType Type { get; set; }

    }
}
