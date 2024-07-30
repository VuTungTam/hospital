using Hospital.Domain.Entities.HeathFacilities;
using Hospital.Domain.Entities.HeathServices;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Specialties
{
    [Table("FacilitySpecialty")]
    public class FacilitySpecialty : 
        BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        public long FacilityId { get; set; }
        public long SpecialtyId { get; set; }
        public HealthFacility Facility { get; set; }
        public Specialty Specialty { get; set; }
        public List<HealthService> Services { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
