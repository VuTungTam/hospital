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
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.Domain.Specifications;

namespace Hospital.Infra.Repositories
{
    public class OrmRepository : IOrmRepository
    {
        protected readonly IServiceProvider _serviceProvider;

        protected readonly AppDbContext _dbContext;

        protected readonly IExecutionContext _executionContext;

        public OrmRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _executionContext = serviceProvider.GetRequiredService<IExecutionContext>();
        }

        public virtual ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec, bool ignoreOwner = false, bool ignoreBranch = false) where T : BaseEntity
        {
            spec ??= new ExpressionSpecification<T>(x => true);

            if (_executionContext.IsAnonymous || typeof(T).HasInterface<ISystemEntity>())
            {
                return spec;
            }

            if (!ignoreBranch && typeof(T).HasInterface<IBranchId>())
            {
                spec = spec.And(new LimitByBranchIdSpecification<T>(_executionContext.BranchId));
            }

            if (!ignoreOwner && typeof(T).HasInterface<IPersonalizeEntity>())
            {
                spec = spec.And(new LimitByOwnerIdSpecification<T>(_executionContext.UserId));
            }

            return spec;
        }

        public virtual async Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, string[] includes = default, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsNoTracking();

            spec = GuardDataAccess(spec, ignoreOwner, ignoreBranch);

            if (includes != null && includes.Any())
            {
                query = query.IncludesRelateData(includes);
            }

            return await query.FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public virtual async Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, string[] includes = default, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsNoTracking();

            spec = GuardDataAccess(spec, ignoreOwner, ignoreBranch);

            if (includes != null && includes.Any())
            {
                query = query.IncludesRelateData(includes);
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
    }
}
