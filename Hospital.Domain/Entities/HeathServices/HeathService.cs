using Hospital.Domain.Entities.HeathFacilities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.HeathServices
{
    public class HeathService
      : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeleteBy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long TypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        public long FacilityId { get; set; }
        public HealthFacility HealthFacility { get; set; }
        public decimal Price { get; set; }
        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
