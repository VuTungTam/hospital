using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Microsoft.Extensions.Localization;
using Hospital.Resource.Properties;

namespace Hospital.Infra.Repositories
{
    public class WriteRepository<T> : OrmRepository, IWriteRepository<T>, IDisposable where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IStringLocalizer<Resources> _localizer;
        public WriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer
            ) : base(serviceProvider)
        {
            _dbSet = _dbContext.Set<T>();
            _localizer = localizer;
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

        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            AddRange(entities);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            if (typeof(T).HasInterface<ISoftDelete>())
            {
                var dateService = _serviceProvider.GetRequiredService<IDateService>();
                foreach( var entity in entities)
                {
                    (entity as ISoftDelete).Deleted = dateService.GetClientTime();
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
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task UpdateAsync(T entity, List<string> specificUpdates = null, CancellationToken cancellationToken = default)
        {
            Update(entity);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            UpdateRange(entities);
            
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
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
    }
}
