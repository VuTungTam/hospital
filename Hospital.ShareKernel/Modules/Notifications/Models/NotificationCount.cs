namespace Hospital.SharedKernel.Modules.Notifications.Models
{
    public class NotificationCount
    {
        public int Total { get; set; }

        public int Read { get; set; }

        public int Unread => Total - Read;
    }
}
