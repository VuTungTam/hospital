using Hospital.Domain.Entities.QueueItems;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Queue
{
    public interface IQueueItemReadRepository : IReadRepository<QueueItem>
    {
        Task<int> GetQuantityTodayAsync(CancellationToken cancellationToken);
        Task<int> GetCurrentPositionAsync(CancellationToken cancellationToken);
    }
}
