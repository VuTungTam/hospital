using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.Specialties
{
    [Table("tbl_specialties")]
    public class Specialty
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

        public string Symptoms { get; set; }

        public List<HealthService> HealthServices { get; set; }

        public List<FacilitySpecialty> FacilitySpecialties { get; set; }

        public List<ZoneSpecialty> ZoneSpecialties { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
