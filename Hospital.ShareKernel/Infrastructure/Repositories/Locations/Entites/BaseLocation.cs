using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Enums;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites
{
    public class BaseLocation : BaseEntity
    {
        public string Name { get; set; }
        public LocationType Type { get; set; }
    }
}
