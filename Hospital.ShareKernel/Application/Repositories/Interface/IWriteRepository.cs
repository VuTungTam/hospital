using Hospital.SharedKernel.Domain.Entities.Base;
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

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        void Update(T entity);

        Task UpdateAsync(T entity, List<string> specificUpdates = null, CancellationToken cancellationToken = default);

        void UpdateRange(IEnumerable<T> entities);

        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Delete(IEnumerable<T> entities);

        Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    }
}
