using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
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
        ISoftDelete,
        IDeletedBy
    {
        public long FacilityId { get; set; }
        public long SpecialtyId { get; set; }
        public HealthFacility Facility { get; set; }
        public Specialty Specialty { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public List<HealthService> Services { get; set; }
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
