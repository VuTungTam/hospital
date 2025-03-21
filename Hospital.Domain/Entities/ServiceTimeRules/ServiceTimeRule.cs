using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.ServiceTimeRules
{
    [Table("tbl_service_time_rules")]
    public class ServiceTimeRule
        : BaseEntity,
        IModifiedAt,
        IModifiedBy,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public long ServiceId { get; set; }

        public HealthService Service { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan StartBreakTime { get; set; }

        public TimeSpan EndBreakTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int SlotDuration { get; set; }

        public int MaxPatients { get; set; }

        public List<TimeSlot> TimeSlots { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

    }
}
