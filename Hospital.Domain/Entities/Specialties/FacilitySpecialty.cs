using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Specialties
{
    [Table("tbl_facility_specialty")]
    public class FacilitySpecialty : 
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public long FacilityId { get; set; }

        public long SpecialtyId { get; set; }

        public HealthFacility Facility { get; set; }

        public Specialty Specialty { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
