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

        public async Task<int> GetCurrentPositionAsync(CancellationToken cancellationToken)
        {
            var item = await _dbContext.QueueItems.Where(q => q.State == 1).SingleOrDefaultAsync();
            return item.Position;
        }

        public async Task<int> GetQuantityTodayAsync(CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _dbContext.QueueItems.Where(q => DateOnly.FromDateTime(q.Date) == today).CountAsync(cancellationToken: cancellationToken);
        }
    }
}
