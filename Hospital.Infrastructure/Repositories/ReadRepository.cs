using System.Linq.Expressions;
using System.Reflection;
using Hospital.Domain.Constants;
using Hospital.Domain.Specifications;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Specifications.Interfaces;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;


namespace Hospital.Infrastructure.Repositories
{
    public class ReadRepository<T> : OrmRepository, IReadRepository<T> where T : BaseEntity
    {
        protected readonly QueryOption QueryOption;

        protected readonly DbSet<T> _dbSet;

        protected readonly IStringLocalizer<Resources> _localizer;

        protected readonly IRedisCache _redisCache;

        public ReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider)
        {
            QueryOption = new QueryOption(null, false);

            _dbSet = _dbContext.Set<T>();
            _localizer = localizer;
            _redisCache = redisCache;
        }

        public QueryOption DefaultQueryOption => QueryOption;

        //public override ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec, QueryOption option = default)
        //{
        //    return base.GuardDataAccess(spec, option);
        //}


        #region Paging
        public virtual async Task<PaginationResult<T>> GetPaginationAsync(Pagination pagination, ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default)
        {
            option ??= new QueryOption();
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking().IncludesRelateData(option.Includes), pagination)
                         .Where(guardExpression)
                         .BuildOrderBy(pagination.Sorts);

            var query2 = query.BuildLimit(pagination.Offset, pagination.Size);
            var dataFactory = () => query2.ToListAsync(cancellationToken);
            var countFactory = () => query.CountAsync(cancellationToken);

            if (!option.IgnoreOwner && !option.IgnoreFacility)
            {
                var cacheEntry = CacheManager.GetPaginationCacheEntry<T>(pagination, _executionContext.Identity, _executionContext.FacilityId);
                var valueFactory = async () => new PaginationResult<T>(await dataFactory(), await countFactory());

                //return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            }

            var data = await dataFactory();
            var count = await countFactory();
            return new PaginationResult<T>(data, count);
        }

        protected virtual IQueryable<T> BuildSearchPredicate(IQueryable<T> query, Pagination pagination)
        {
            if (string.IsNullOrEmpty(pagination.Search))
            {
                return query;
            }

            Log.Logger.Information($"Search key word: {typeof(T).Name}: {pagination.Search}");
            var properties = typeof(T).GetProperties().Where(x => x.GetIndexParameters().Length == 0);

            // Here we will collect a single search request for all properties
            Expression body = Expression.Constant(false);

            // Get our generic object
            var parameter = Expression.Parameter(typeof(T), "x");

            // Get the Method from EF.Functions
            var methodName = GetMethodName(pagination.QueryType);
            var efMethod = typeof(DbFunctionsExtensions).GetMethod(methodName,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                null);
            // We make a pattern for the search
            var pattern = Expression.Constant($"%{pagination.Search}%", typeof(string));

            foreach (var prop in properties)
            {
                if (prop.IsDefined(typeof(FilterableAttribute), true))
                {
                    // Get property from our object
                    var property = Expression.Property(parameter, prop.Name);

                    // Сall the method with all the required arguments
                    Expression expr = Expression.Call(efMethod, Expression.Property(null, typeof(EF), nameof(EF.Functions)), property, pattern);

                    // Add to the main request
                    body = Expression.OrElse(body, expr);
                }
            }
            // Compose and pass the expression to Where
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            if (predicate == null)
            {
                return query;
            }
            return query.Where(predicate);

            string GetMethodName(QueryType type)
            {
                switch (type)
                {
                    case QueryType.Contains:
                    case QueryType.NotContain:
                    case QueryType.Equals:
                    case QueryType.NotEquals:
                    case QueryType.GreaterThan:
                    case QueryType.GreaterThanOrEqual:
                    case QueryType.LessThan:
                    case QueryType.LessThanOrEqual:
                    case QueryType.StartsWith:
                    case QueryType.NotStartsWith:
                    case QueryType.EndsWith:
                    case QueryType.NotEndWith:
                        return "Like";
                }
                return "Like";
            }
        }
        #endregion

        public virtual async Task<T> GetByIdAsync(long id, QueryOption option = default, CancellationToken cancellationToken = default)
        {
            option ??= new QueryOption();
            if (option.Includes != null && option.Includes.Any())
            {
                return await FindBySpecificationAsync(new GetByIdSpecification<T>(id), option, cancellationToken);
            }

            var cacheEntry = GetCacheEntry(id);
            var data = await _redisCache.GetAsync<T>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                data = await FindBySpecificationAsync(new GetByIdSpecification<T>(id), option, cancellationToken);
                if (data != null)
                {
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(AppCacheTime.RecordWithId), cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public virtual async Task<List<T>> GetByIdsAsync(IList<long> ids, QueryOption option = default, CancellationToken cancellationToken = default)
            => await GetBySpecificationAsync(new GetByIdsSpecification<T>(ids), option, cancellationToken);

        public virtual async Task<List<T>> GetAsync(ISpecification<T> spec, QueryOption option, CancellationToken cancellationToken = default)
        {
            option ??= new QueryOption();
            if (option.Includes != null && option.Includes.Any())
            {
                return await GetBySpecificationAsync(spec, option, cancellationToken);
            }

            var cacheEntry = GetCacheEntry(type: "all");
            var data = await _redisCache.GetAsync<List<T>>(cacheEntry.Key, cancellationToken: cancellationToken);
            if (data == null)
            {
                data = await GetBySpecificationAsync(spec, option, cancellationToken);
                if (data != null && data.Any())
                {
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(AppCacheTime.Records), cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> GetCountBySpecAsync(ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default)
        {
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = _dbSet.AsNoTracking().Where(guardExpression);
            return await query.CountAsync(cancellationToken);
        }
        protected CacheEntry GetCacheEntry(long id = 0, string type = "")
        {
            if (type == "all")
            {
                if (typeof(T).HasInterface<IOwnedEntity>())
                {
                    return CacheManager.DbOwnerAllCacheEntry<T>(_executionContext.Identity);
                }
                return CacheManager.DbSystemAllCacheEntry<T>();
            }

            if (typeof(T).HasInterface<IOwnedEntity>())
            {
                return CacheManager.DbOwnerIdCacheEntry<T>(id, _executionContext.Identity);
            }
            return CacheManager.DbSystemIdCacheEntry<T>(id);
        }


    }
}
