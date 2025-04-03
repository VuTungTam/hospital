using Hospital.Application.Dtos;
using Hospital.SharedKernel.Modules.Notifications.Enums;

namespace Hospital.Application.Dtos.Notifications
{
    public class NotificationDto : BaseDto
    {
        public NotificationType Type { get; set; }

        public bool IsUnread { get; set; }

        public string Data { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
