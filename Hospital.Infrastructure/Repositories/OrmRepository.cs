using Microsoft.Extensions.DependencyInjection;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Repositories.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications.Interfaces;
using System.Threading;
using System;
using Hospital.Infra.EFConfigurations;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Hospital.Infrastructure.Extensions;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Infra.Repositories
{
    public class OrmRepository : IOrmRepository
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly AppDbContext _dbContext;
        public OrmRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            
        }

        public virtual async Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsQueryable();

            if (option?.Includes != null)
            {
                query = query.IncludesRelateData(option.Includes.ToArray());
            }

            if (option == null || option.AsNoTracking == true)
            {
                query = query.AsNoTracking();
            }

            if (option == null || option.Guard)
            {
                spec = GuardDataAccess(spec);
            }

            return await query.FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public virtual async Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            var DbSet = _dbContext.Set<T>();
            var query = DbSet.AsQueryable();

            if (option == null || option.AsNoTracking == true)
            {
                query = query.AsNoTracking();
            }

            if (option?.Includes != null)
            {
                query = query.IncludesRelateData(option.Includes.ToArray());
            }

            if (option == null || option.Guard)
            {
                spec = GuardDataAccess(spec);
            }

            if (typeof(T).HasInterface<IModified>())
            {
                if (typeof(T).HasInterface<ICreated>())
                {
                    query = query.OrderByDescending(x => (x as IModified).Modified ?? (x as ICreated).Created);
                }
                else
                {
                    query = query.OrderByDescending(x => (x as IModified).Modified);
                }
            }

            return await query.Where(spec.GetExpression()).ToListAsync(cancellationToken);
        }

        public virtual ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec) where T : BaseEntity
        {
            spec ??= new ExpressionSpecification<T>(x => true);
            return spec;
        }
    }
}
