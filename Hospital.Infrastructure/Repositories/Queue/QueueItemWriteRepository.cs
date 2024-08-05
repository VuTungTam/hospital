using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Queue
{
    public class QueueItemWriteRepository : WriteRepository<QueueItem>, IQueueItemWriteRepository
    {
        public QueueItemWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }

    }
}
