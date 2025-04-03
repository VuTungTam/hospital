using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Modules.Notifications.Entities
{
    [Table("mcs_notifications")]
    public class Notification :
        BaseEntity,
        IOwnedEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        ISoftDelete,
        IDeletedBy
    {
        public NotificationType Type { get; set; }

        public bool IsUnread { get; set; } = true;

        public string Data { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

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
