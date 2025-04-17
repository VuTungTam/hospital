using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Images;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthFacilities
{
    [Table("tbl_health_facilities")]
    public class HealthFacility
      : BaseEntity,
        IModifiedAt,
        IModifiedBy,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string SummaryVn { get; set; }

        public string SummaryEn { get; set; }

        public string Image { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public HealthFacilityStatus Status { get; set; }

        public List<FacilitySpecialty> FacilitySpecialties { get; set; }

        public List<Image> Images { get; set; }

        public List<Zone> Zones { get; set; }

        public List<HealthService> HealthServices { get; set; }

        public List<FacilityTypeMapping> FacilityTypeMappings { get; set; }
        public int Pid { get; set; }

        public string Pname { get; set; }

        public int Did { get; set; }

        public string Dname { get; set; }

        public int Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string MapURL { get; set; }

        public float StarPoint { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public string Slug { get; set; }
    }
}
