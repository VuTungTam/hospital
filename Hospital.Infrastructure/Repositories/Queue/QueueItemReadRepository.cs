using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Polly;

namespace Hospital.Infrastructure.Repositories.Queue
{
    public class QueueItemReadRepository : ReadRepository<QueueItem>, IQueueItemReadRepository
    {
        public QueueItemReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<List<QueueItem>> GetByDateAsync(long serviceId, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.QueueItems.Where(q => q.Created.Date == date.Date && q.Booking.ServiceId == serviceId).ToListAsync(cancellationToken);
        }

        public async Task<QueueItem> GetByPositionAsync(long serviceId, int position, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.QueueItems.Where(q => q.Position == position && q.Created.Date == date.Date && q.Booking.ServiceId == serviceId).SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<QueueItem> GetCurrentAsync(long serviceId, CancellationToken cancellationToken)
        {
            var item = await _dbContext.QueueItems.Where(q => q.State == 1 && q.Created.Date == DateTime.Now.Date && q.Booking.ServiceId == serviceId).SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return item;
        }
        public async Task<int> GetQuantityTodayAsync(long serviceId, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            return await _dbContext.QueueItems.Where(q => q.Created.Date == today && q.Booking.ServiceId == serviceId).CountAsync(cancellationToken: cancellationToken);
        }
    }
}
