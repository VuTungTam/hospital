using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Specifications;
using Hospital.Infra.EFConfigurations;
using Hospital.Infrastructure.Extensions;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.Repositories
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

        public virtual ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec, QueryOption option = default) where T : BaseEntity
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<T>(x => true);

            if (_executionContext.IsAnonymous || typeof(T).HasInterface<ISystemEntity>())
            {
                return spec;
            }
            if (!option.IgnoreFacility && typeof(T).HasInterface<IFacility>())
            {
                spec = spec.And(new LimitByFacilityIdSpecification<T>(_executionContext.FacilityId));

            }
            if (!option.IgnoreZone && typeof(T).HasInterface<IZone>())
            {
                if(_executionContext.ZoneId == 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<T>(_executionContext.FacilityId));
                }
                else
                {
                    spec = spec.And(new LimitByZoneIdSpecification<T>(_executionContext.ZoneId));
                }
            }
            if (!option.IgnoreDoctor && typeof(T).HasInterface<IDoctor>())
            {
                spec = spec.And(new LimitByDoctorIdSpecification<T>(_executionContext.Identity));
            }

            if (!option.IgnoreOwner && typeof(T).HasInterface<IOwnedEntity>())
            {
                spec = spec.And(new LimitByOwnerIdSpecification<T>(_executionContext.Identity));
            }

            return spec;
        }

        public virtual async Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            option ??= new QueryOption();

            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsNoTracking();

            spec = GuardDataAccess(spec, option);

            if (option.Includes != null && option.Includes.Any())
            {
                query = query.IncludesRelateData(option.Includes);
            }

            return await query.FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public virtual async Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            option ??= new QueryOption();

            var dbSet = _dbContext.Set<T>();
            var query = dbSet.AsNoTracking();

            spec = GuardDataAccess(spec, option);

            if (option.Includes != null && option.Includes.Any())
            {
                query = query.IncludesRelateData(option.Includes);
            }

            if (typeof(T).HasInterface<IModifiedAt>())
            {
                if (typeof(T).HasInterface<ICreatedAt>())
                {
                    query = query.OrderByDescending(x => (x as IModifiedAt).ModifiedAt ?? (x as ICreatedAt).CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(x => (x as IModifiedAt).ModifiedAt);
                }
            }

            return await query.Where(spec.GetExpression()).ToListAsync(cancellationToken);
        }
    }
}
