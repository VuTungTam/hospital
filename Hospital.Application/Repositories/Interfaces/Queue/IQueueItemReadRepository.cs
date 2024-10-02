using Hospital.Domain.Entities.QueueItems;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Queue
{
    public interface IQueueItemReadRepository : IReadRepository<QueueItem>
    {
        Task<int> GetQuantityTodayAsync(long serviceId,CancellationToken cancellationToken);
        Task<QueueItem> GetCurrentAsync(long serviceId, CancellationToken cancellationToken);
        Task<QueueItem> GetByPositionAsync(long serviceId, int position, DateTime date, CancellationToken cancellationToken);
        Task<List<QueueItem>> GetByDateAsync(long serviceId, DateTime date, CancellationToken cancellationToken);
    }
}
