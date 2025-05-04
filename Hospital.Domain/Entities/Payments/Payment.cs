
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
        ISoftDelete,
        IFacility
    {
        public long BookingId { get; set; }

        public Booking Booking { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public string TransactionContent { get; set; }

        public string PaymentUrl { get; set; }

        public bool IsPaid { get; set; }

        public string ExternalTransactionId { get; set; }

        public string BankBin { get; set; }

        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long FacilityId { get; set; }
    }
}