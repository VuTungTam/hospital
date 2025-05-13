using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using IdGen;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories
{
    public class WriteRepository<T> : OrmRepository, IWriteRepository<T>, IDisposable where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected readonly IRedisCache _redisCache;
        public WriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider)
        {
            _dbSet = _dbContext.Set<T>();
            _localizer = localizer;
            _redisCache = redisCache;
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.Database.BeginTransaction();

            }
        }
        public IUnitOfWork UnitOfWork => _dbContext;
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            Add(entity);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await RemoveCacheWhenAddAsync(entity, cancellationToken);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            AddRange(entities);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
            //await RemoveCacheWhenAddAsync(cancellationToken);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            if (typeof(T).HasInterface<ISoftDelete>())
            {
                var dateService = _serviceProvider.GetRequiredService<IDateService>();
                foreach (var entity in entities)
                {
                    (entity as ISoftDelete).IsDeleted = true;
                    (entity as ISoftDelete).DeletedAt = dateService.GetClientTime();
                }
                _dbSet.UpdateRange(entities);
            }
            else
            {
                _dbSet.RemoveRange(entities);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            Delete(entities);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await RemoveCacheWhenDeleteAsync(entities, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task UpdateAsync(T entity, List<string> excludes = null, CancellationToken cancellationToken = default)
        {
            var cacheEntry = CacheManager.GetLockUpdateCacheEntry<T>(entity.Id);
            var locked = await _redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken);
            if (!string.IsNullOrEmpty(locked))
            {
                throw new BadRequestException(_localizer["CommonMessage.RecordIsUpdating"]);
            }

            try
            {
                await _redisCache.SetAsync(cacheEntry.Key, "locked", TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);

                Update(entity);
                var properties = entity.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0 && p.PropertyType.IsPrimitive());

                excludes ??= new();

                var entry = _dbContext.Entry(entity);
                foreach (var property in properties)
                {
                    if (property.GetCustomAttributes(typeof(ImmutableAttribute), true).Any())
                    {
                        entry.Property(property.Name).IsModified = false;
                        continue;
                    }

                    if (excludes.Contains(property.Name))
                    {
                        entry.Property(property.Name).IsModified = false;
                    }
                }

                await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

                await RemoveCacheWhenUpdateAsync(entity.Id, cancellationToken);
            }
            finally
            {
                await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
            }

        }

        #region Dispose
        public void Dispose()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                _dbContext.Database.CurrentTransaction.Dispose();
            }
        }
        #endregion
        public virtual async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
        }

        public virtual async Task<CacheEntry> SetBlockUpdateCacheAsync(long id, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetLockUpdateCacheEntry<T>(id);
            var locked = await _redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken);
            if (!string.IsNullOrEmpty(locked))
            {
                throw new BadRequestException(_localizer["CommonMessage.RecordIsUpdating"]);
            }
            await _redisCache.SetAsync(cacheEntry.Key, "locked", TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            return cacheEntry;
        }

        public virtual async Task<List<CacheEntry>> SetBlockUpdateRangeCacheAsync(List<long> ids, CancellationToken cancellationToken)
        {
            var cacheEntries = new List<CacheEntry>();
            var getTasks = new List<Task<string>>();

            foreach (var id in ids)
            {
                var cacheEntry = CacheManager.GetLockUpdateCacheEntry<T>(id);
                cacheEntries.Add(cacheEntry);
                getTasks.Add(_redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken));
            }

            var lockedValues = await Task.WhenAll(getTasks);

            if (lockedValues.Any(value => !string.IsNullOrEmpty(value)))
            {
                throw new BadRequestException(_localizer["CommonMessage.RecordIsUpdating"]);
            }

            var setTasks = new List<Task>();

            foreach (var cacheEntry in cacheEntries)
            {
                setTasks.Add(_redisCache.SetAsync(cacheEntry.Key, "locked",
                    TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds),
                    cancellationToken: cancellationToken));
            }

            await Task.WhenAll(setTasks);

            return cacheEntries;
        }


        #region Remove Cache
        private static readonly bool _isOwnedEntity = typeof(T).HasInterface<IOwnedEntity>();

        public virtual async Task RemoveOneRecordCacheAsync(long id, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.DbSystemIdCacheEntry<T>(id);

            if (_isOwnedEntity)
            {
                await RemoveOwnerIdCache(id, cancellationToken);
            }

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
        }

        public async Task RemoveOwnerIdCache(long id, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var cacheEntry = CacheManager.DbSystemIdCacheEntry<T>(id);
            var data = await _redisCache.GetAsync<T>(cacheEntry.Key, cancellationToken);
            data ??= await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (data is IOwnedEntity owned)
            {
                var cacheEntry2 = CacheManager.DbOwnerIdCacheEntry<T>(id, owned.OwnerId);
                var cacheEntry3 = CacheManager.DbOwnerAllCacheEntry<T>(owned.OwnerId);
                tasks.Add(_redisCache.RemoveAsync(cacheEntry2.Key, cancellationToken));
                tasks.Add(RemoveAllOwnerCache(owned.OwnerId, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        public async Task RemoveAllOwnerCache(long ownerId, CancellationToken cancellationToken)
        {
            var cacheEntry3 = CacheManager.DbOwnerAllCacheEntry<T>(ownerId);
            await _redisCache.RemoveAsync(cacheEntry3.Key, cancellationToken);
        }

        protected virtual async Task RemoveAllRecordsCacheAsync(CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.DbSystemAllCacheEntry<T>();

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
        }

        protected virtual async Task RemovePaginationCacheAsync(CancellationToken cancellationToken)
        {
            var key = CacheManager.GetRemovePaginationPattern<T>(_executionContext.Identity, _executionContext.FacilityId);
            await _redisCache.RemoveByPatternAsync(key);
        }

        public virtual async Task RemoveCacheWhenAddAsync(T entity, CancellationToken cancellationToken)
        {
            if (_isOwnedEntity)
            {
                await RemoveAllOwnerCache((entity as IOwnedEntity).OwnerId, cancellationToken);
            }
            await RemoveAllRecordsCacheAsync(cancellationToken);
            await RemovePaginationCacheAsync(cancellationToken);
        }

        public virtual async Task RemoveCacheWhenUpdateAsync(long id, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>
                {
                    RemoveAllRecordsCacheAsync(cancellationToken),
                    RemoveOneRecordCacheAsync(id, cancellationToken),
                    RemovePaginationCacheAsync(cancellationToken)
                };
            await Task.WhenAll(tasks);
        }

        public virtual async Task RemoveCacheWhenDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (_isOwnedEntity)
            {
                var ids = entities.Select(x => x.Id).ToList();
                var ownerIds = entities
                    .Select(x => (x as IOwnedEntity).OwnerId)
                    .Distinct()
                    .ToList();
                foreach (var id in ids)
                {
                    await RemoveOwnerIdCache(id, cancellationToken);
                }
                foreach (var ownerId in ownerIds)
                {
                    await RemoveAllOwnerCache(ownerId, cancellationToken);
                }
            }
            await RemoveAllRecordsCacheAsync(cancellationToken);
            await RemovePaginationCacheAsync(cancellationToken);
        }
        #endregion
    }
}
