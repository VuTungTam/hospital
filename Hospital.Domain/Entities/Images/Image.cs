using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.Images
{
    [Table("tbl_images")]
    public class Image :
        BaseEntity
    {
        public HealthFacility HealthFacility { get; set; }

        public long FacilityId { get; set; }

        public string PublicId { get; set; }

    }
}
