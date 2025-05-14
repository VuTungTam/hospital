using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Payments;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Domain.Entities.Bookings
{
    [Table("tbl_bookings")]
    public class Booking : BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        ISoftDelete,
        IDeletedBy,
        IOwnedEntity,
        IFacility,
        IZone,
        IDoctor
    {
        [Filterable("Mã lịch khám")]
        [Immutable]
        public string Code { get; set; }

        public long HealthProfileId { get; set; }

        [Filterable("Tên người đặt")]
        public string HealthProfileName { get; set; }

        public HealthProfile HealthProfile { get; set; }

        public DateTime Date { get; set; }

        public BookingStatus Status { get; set; }

        public long ServiceId { get; set; }

        public HealthService Service { get; set; }

        public long TimeSlotId { get; set; }

        public TimeSpan StartBooking { get; set; }

        public TimeSpan EndBooking { get; set; }

        public TimeSlot TimeSlot { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int Order { get; set; }

        public bool IsFeedbacked { get; set; } = false;

        public Feedback Feedback { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public long OwnerId { get; set; }

        public long FacilityId { get; set; }

        public string FacilityName { get; set; }

        public HealthFacility HealthFacility { get; set; }

        public long ZoneId { get; set; }

        public long DoctorId { get; set; }

        public List<Payment> Payments { get; set; }

        public CancelReason CancelReason { get; set; }
    }
}
