using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Modules.Notifications.Entities;

namespace Hospital.SharedKernel.Modules.Notifications.Interfaces
{
    public interface INotificationWriteRepository : IWriteRepository<Notification>
    {
        Task RemovePaginationCacheByUserIdAsync(long userId, CancellationToken cancellationToken);

        Task MarkAsReadOrUnreadAsync(long id, bool markAsRead, CancellationToken cancellationToken);

        Task DeleteAsync(List<long> ids, CancellationToken cancellationToken);

        Task MarkAllAsReadAsync(CancellationToken cancellationToken);
    }
}
