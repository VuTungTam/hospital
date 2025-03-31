using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Feedbacks
{
    [Table("tbl_feedbacks")]
    public class Feedback :
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        IOwnedEntity,
        IFacility
    {

        public int Stars { get; set; }

        public string Message { get; set; }
        [Immutable]
        public long BookingId {  get; set; }

        public Booking Booking { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public long OwnerId { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }
        [Filterable("Mã lịch khám")]
        [Immutable]
        public string BookingCode { get; set; }

        public long FacilityId { get; set; }
    }
}
