using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.FacilityTypes
{
    [Table("tbl_facility_types")]
    public class FacilityType
      : BaseEntity,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public List<FacilityTypeMapping> FacilityTypeMappings { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
