using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.TimeSlots
{
    [Table("tbl_time_slots")]
    public class TimeSlot :
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public long TimeRuleId { get; set; }

        public ServiceTimeRule ServiceTimeRule { get; set; }

        public bool ForWalkin { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

    }
}
