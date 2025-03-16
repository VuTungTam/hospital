using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthProfiles
{
    [Table("tbl_health_profiles")]
    public class HealthProfile
        : BaseEntity,
        IModifiedAt,
        IModifiedBy,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy,
        IOwnedEntity
    {
        public string CICode { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int Gender { get; set; }

        public DateTime Dob { get; set; }

        public int Pid { get; set; }

        public string Pname { get; set; }

        public int Did { get; set; }

        public string Dname { get; set; }

        public int Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public int Eid { get; set; }

        public string Ethinic { get; set; }

        public List<Booking> Bookings { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public long OwnerId { get ; set; }
    }
}
