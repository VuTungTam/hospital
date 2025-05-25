
using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.Payments
{
    [Table("tbl_payment")]
    public class Payment
        : BaseEntity,
        ICreatedAt,
        IFacility,
        IOwnedEntity
    {
        public long BookingId { get; set; }

        public Booking Booking { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public string TransactionContent { get; set; }

        public long TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long FacilityId { get; set; }

        public long OwnerId { get; set; }
    }
}