using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        IOwnedEntity
    {
        [Filterable("Mã lịch khám")]
        [Immutable]
        public string Code { get; set; }

        public long HealthProfileId { get; set; }

        public HealthProfile HealthProfile { get; set; }

        public DateTime Date { get; set; }

        public BookingStatus Status { get; set; }

        public long ServiceId { get; set; }

        public HealthService Service { get; set; }

        public long TimeSlotId { get; set; }

        public TimeSlot TimeSlot { get; set; }

        public int Order {  get; set; }
        public bool IsFeedbacked { get; set; } = false; 

        public Feedback Feedback { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<BookingSymptom> BookingSymptoms { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public long OwnerId { get; set; }

    }
}
