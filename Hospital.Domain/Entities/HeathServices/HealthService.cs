using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.HeathFacilities;
using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HeathServices
{
    [Table("HealthServices")]
    public class HealthService
      : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long TypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        public long FacilitySpecialtyId { get; set; }
        public FacilitySpecialty FacilitySpecialty { get; set; }
        public List<Declaration> Declarations { get; set; }
        public decimal Price { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
