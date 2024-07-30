using Hospital.Domain.Entities.QueueItems;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Queue
{
    public interface IQueueItemWriteRepository : IWriteRepository<QueueItem>
    {
    }
}
