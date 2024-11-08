using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.ServiceTimeRules
{
    [Table("ServiceTimeRules")]
    public class ServiceTimeRule
        : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
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

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }

        

    }
}
