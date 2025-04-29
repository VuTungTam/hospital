using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.FacilityTypes
{
    [Table("tbl_facility_types")]
    public class FacilityType
      : BaseEntity
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string Slug { get; set; }

        public List<FacilityTypeMapping> FacilityTypeMappings { get; set; }
    }
}
