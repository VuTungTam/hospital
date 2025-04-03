using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Models;

namespace Hospital.SharedKernel.Modules.Notifications.Interfaces
{
    public interface INotificationReadRepository : IReadRepository<Notification>
    {
        Task<NotificationCount> GetCountAsync(CancellationToken cancellationToken);
    }
}
