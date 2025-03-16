using Hospital.Domain.Entities.FacilityTypes;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthFacilities
{
    [Table("tbl_facility_type_mappings")]
    public class FacilityTypeMapping :
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public long FacilityId { get; set; }

        public long TypeId { get; set; }

        public HealthFacility Facility { get; set; }

        public FacilityType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
