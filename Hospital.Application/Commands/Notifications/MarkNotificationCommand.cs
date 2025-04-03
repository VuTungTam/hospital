using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Notifications
{
    public class MarkNotificationCommand : BaseCommand
    {
        public MarkNotificationCommand(long id, bool markAsRead)
        {
            Id = id;
            MarkAsRead = markAsRead;
        }

        public long Id { get; }
        public bool MarkAsRead { get; }
    }
}
