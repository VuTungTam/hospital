using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Zones
{
    [Table("tbl_zones")]
    public class Zone
      : BaseEntity,
        IModifiedAt,
        IModifiedBy,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy,
        IFacility
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public List<ZoneSpecialty> ZoneSpecialties { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public long FacilityId { get ; set ; }

        public HealthFacility HealthFacility { get; set; }
    }
}
