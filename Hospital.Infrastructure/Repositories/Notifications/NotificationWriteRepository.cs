using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Dapper;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Notifications
{
    public class NotificationWriteRepository : WriteRepository<Notification>, INotificationWriteRepository
    {
        private readonly IDbConnection _dbConnection;

        public NotificationWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IDbConnection dbConnection
        ) : base(serviceProvider, localizer, redisCache)
        {
            _dbConnection = dbConnection;
        }

        public async Task MarkAllAsReadAsync(CancellationToken cancellationToken)
        {
            var updateColumns = $"IsUnread = 0, ModifiedAt = CURRENT_TIMESTAMP(), ModifiedBy = {_executionContext.Identity}";
            var sql = $"UPDATE {new Notification().GetTableName()} SET {updateColumns} WHERE OwnerId = {_executionContext.Identity} AND IsDeleted = 0";

            await _dbConnection.ExecuteAsync(sql, null, autoCommit: true);
            await RemovePaginationCacheByUserIdAsync(_executionContext.Identity, cancellationToken);
        }

        public async Task MarkAsReadOrUnreadAsync(long id, bool markAsRead, CancellationToken cancellationToken)
        {
            var updateColumns = $"IsUnread = {!markAsRead}, ModifiedAt = CURRENT_TIMESTAMP(), ModifiedBy = {_executionContext.Identity}";
            var sql = $"UPDATE {new Notification().GetTableName()} SET {updateColumns} WHERE Id = {id} AND OwnerId = {_executionContext.Identity} AND IsDeleted = 0";

            await _dbConnection.ExecuteAsync(sql, null, autoCommit: true);
            await RemovePaginationCacheByUserIdAsync(_executionContext.Identity, cancellationToken);
        }

        public async Task DeleteAsync(List<long> ids, CancellationToken cancellationToken)
        {
            var sql = $"UPDATE {new Notification().GetTableName()} SET IsDeleted = 1, DeletedAt = CURRENT_TIMESTAMP(), DeletedBy = {_executionContext.Identity} WHERE Id IN ({string.Join(",", ids)}) AND {nameof(Notification.OwnerId)} = {_executionContext.Identity} AND IsDeleted = 0";

            await _dbConnection.ExecuteAsync(sql, null, autoCommit: true);
            await RemovePaginationCacheByUserIdAsync(_executionContext.Identity, cancellationToken);
        }

        public async Task RemovePaginationCacheByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetPaginationCacheEntry<Notification>(new Pagination(1, 1), userId, 0);
            var cacheEntry2 = AppCacheManager.GetNotificationCountCacheEntry(userId);
            var segments = cacheEntry.Key.Split(":");
            var pattern = $"*{segments[0]}:{segments[1]}:{segments[2]}*";

            await _redisCache.RemoveByPatternAsync(pattern);
            await _redisCache.RemoveAsync(cacheEntry2.Key, cancellationToken);
        }
    }
}
