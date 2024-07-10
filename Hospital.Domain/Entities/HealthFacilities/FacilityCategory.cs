using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using MassTransit.Futures.Contracts;

namespace Hospital.Domain.Entities.HeathFacilities
{
    public class FacilityCategory 
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

        public List<HealthFacility> Facilities { get; set; }

        public DateTime? Modified { get ; set; }

        public long? Modifier { get ; set ; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
