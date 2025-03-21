using Hospital.Domain.Entities.FacilityTypes;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthFacilities
{
    [Table("tbl_facility_type_mappings")]
    public class FacilityTypeMapping :
        BaseEntity
    {
        public long FacilityId { get; set; }

        public long TypeId { get; set; }

        public HealthFacility Facility { get; set; }

        public FacilityType Type { get; set; }
    }
}
