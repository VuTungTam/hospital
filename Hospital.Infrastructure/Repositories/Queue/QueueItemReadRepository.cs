using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Polly;

namespace Hospital.Infrastructure.Repositories.Queue
{
    public class QueueItemReadRepository : ReadRepository<QueueItem>, IQueueItemReadRepository
    {
        public QueueItemReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }

        public async Task<List<QueueItem>> GetByDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.QueueItems.Where(q => q.Created.Date == date.Date).ToListAsync(cancellationToken);
        }

        public async Task<QueueItem> GetByPositionAsync(int position, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.QueueItems.Where(q => q.Position == position && q.Created.Date == date.Date).SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<QueueItem> GetCurrentAsync(CancellationToken cancellationToken)
        {
            var item = await _dbContext.QueueItems.Where(q => q.State == 1 && q.Created.Date == DateTime.Now.Date).SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return item;
        }

        public async Task<int> GetQuantityTodayAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            return await _dbContext.QueueItems.Where(q => q.Created.Date == today).CountAsync(cancellationToken: cancellationToken);
        }
    }
}
