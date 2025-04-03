using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Modules.Notifications.Models;

namespace Hospital.Application.Queries.Notifications
{
    public class GetNotificationsCountQuery : BaseQuery<NotificationCount>
    {
    }
}
