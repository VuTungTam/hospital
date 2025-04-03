using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Notifications
{
    public class DeleteNotificationsCommand : BaseCommand
    {
        public DeleteNotificationsCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
