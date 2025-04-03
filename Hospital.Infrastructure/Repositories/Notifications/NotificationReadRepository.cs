using Hospital.Domain.Constants;
using Hospital.Domain.Specifications;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Models;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Notifications
{
    public class NotificationReadRepository : ReadRepository<Notification>, INotificationReadRepository
    {
        public NotificationReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<NotificationCount> GetCountAsync(CancellationToken cancellationToken)
        {
            var cacheEntry = AppCacheManager.GetNotificationCountCacheEntry(_executionContext.Identity);
            var count = await _redisCache.GetAsync<NotificationCount>(cacheEntry.Key, cancellationToken);
            if (count != null)
            {
                return count;
            }

            var spec = new OwnerIdEqualsSpecification<Notification>(_executionContext.Identity);
            var total = await _dbSet.AsNoTracking().Where(spec.GetExpression()).Select(x => !x.IsUnread).ToListAsync(cancellationToken);
            if (total.Any())
            {
                count = new NotificationCount
                {
                    Total = total.Count,
                    Read = total.Count(b => b),
                };

                await _redisCache.SetAsync(cacheEntry.Key, count, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            }

            return count ?? new();
        }

        public override async Task<PaginationResult<Notification>> GetPaginationAsync(Pagination pagination, ISpecification<Notification> spec, QueryOption option = default, CancellationToken cancellationToken = default)
        {
            option ??= new QueryOption();

            var expression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(expression)
                         .OrderByDescending(x => x.Timestamp);

            var query2 = query.BuildLimit(pagination.Offset, pagination.Size);
            var dataFactory = () => query2.ToListAsync(cancellationToken);
            var countFactory = () => query.CountAsync(x => x.IsUnread, cancellationToken);

            if (!option.IgnoreOwner && !option.IgnoreFacility)
            {
                var cacheEntry = CacheManager.GetPaginationCacheEntry<Notification>(pagination, _executionContext.Identity, _executionContext.FacilityId);
                var valueFactory = async () => new PaginationResult<Notification>(await dataFactory(), await countFactory());

                return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            }

            var data = await dataFactory();
            var count = await countFactory();

            return new PaginationResult<Notification>(data, count);
        }
    }
}
