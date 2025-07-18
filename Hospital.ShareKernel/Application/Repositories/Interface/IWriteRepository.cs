﻿using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;

namespace Hospital.SharedKernel.Application.Repositories.Interface
{
    public interface IWriteRepository<T> : IOrmRepository where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        void Add(T entity);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        void AddRange(IEnumerable<T> entities);

        void UpdateRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        void Update(T entity);

        Task UpdateAsync(T entity, List<string> excludes = null, CancellationToken cancellationToken = default);

        void Delete(IEnumerable<T> entities);

        Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);

        Task<CacheEntry> SetBlockUpdateCacheAsync(long id, CancellationToken cancellationToken);

        Task<List<CacheEntry>> SetBlockUpdateRangeCacheAsync(List<long> ids, CancellationToken cancellationToken);

        Task RemoveCacheWhenAddAsync(T entity, CancellationToken cancellationToken);

        Task RemoveCacheWhenUpdateAsync(long id, CancellationToken cancellationToken);

        Task RemoveCacheWhenDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        Task RemoveAllOwnerCache(long ownerId, CancellationToken cancellationToken);

        Task RemoveOneRecordCacheAsync(long id, CancellationToken cancellationToken);

    }
}
