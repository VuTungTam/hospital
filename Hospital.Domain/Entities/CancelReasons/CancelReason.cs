using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.CancelReasons
{
    [Table("tbl_cancel_reasons")]
    public class CancelReason :
        BaseEntity,
        ICreatedBy
    {
        public string Reason { get; set; }

        public long BookingId { get; set; }

        public Booking Booking { get; set; }

        public CancelType CancelType { get; set; }

        public long? CreatedBy { get; set; }
    }
}
