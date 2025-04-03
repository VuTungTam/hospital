using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Zones
{
    [Table("tbl_zone_specialty")]
    public class ZoneSpecialty : BaseEntity
    {
        public Zone Zone { get; set; }

        public Specialty Specialty { get; set; }

        public long ZoneId { get; set; }

        public long SpecialtyId { get; set; }
    }
}
