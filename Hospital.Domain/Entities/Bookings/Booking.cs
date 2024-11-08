using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Bookings
{
    [Table("Bookings")]
    public class Booking : BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        [Filterable("Mã lịch khám")]
        public string Code { get; set; }

        public long HealthProfileId { get; set; }

        public HealthProfile HealthProfile { get; set; }

        public DateTime Date { get; set; }

        public BookingStatus Status { get; set; }

        public long ServiceId { get; set; }

        public HealthService Service { get; set; }

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public int Order {  get; set; }

        public List<BookingSymptom> BookingSymptoms { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
