using Hospital.Domain.Entities.QueueItems;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Queue
{
    public interface IQueueItemReadRepository : IReadRepository<QueueItem>
    {
        Task<int> GetQuantityTodayAsync(CancellationToken cancellationToken);
        Task<QueueItem> GetCurrentAsync(CancellationToken cancellationToken);
        Task<QueueItem> GetByPositionAsync(int position, DateTime date, CancellationToken cancellationToken);
        Task<List<QueueItem>> GetByDateAsync(DateTime date, CancellationToken cancellationToken);
    }
}
