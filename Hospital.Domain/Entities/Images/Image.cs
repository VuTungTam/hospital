using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Images
{
    [Table("tbl_images")]
    public class Image : 
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IDeletedBy,
        ISoftDelete
    {
        public HealthFacility HealthFacility { get; set; }

        public long FacilityId { get; set; }

        public string Url { get; set; }

        public DateTime CreatedAt { get ; set; }

        public long? CreatedBy { get; set; }

        public long? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
