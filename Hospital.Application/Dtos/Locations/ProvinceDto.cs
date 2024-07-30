using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Enums;

namespace Hospital.Application.Dtos.Locations
{
    public class ProvinceDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public LocationType Type { get; set; }
    }
}
