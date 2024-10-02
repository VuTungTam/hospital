using Hospital.Domain.Constants;
using Hospital.Domain.Specifications;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Repositories.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications.Interfaces;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infra.Repositories
{
    public class ReadRepository<T> : OrmRepository, IReadRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected readonly IRedisCache _redisCache;

        public ReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider)
        {
            _dbSet = _dbContext.Set<T>();
            _localizer = localizer;
            _redisCache = redisCache;
        }

        #region Paging
        public virtual async Task<PagingResult<T>> GetPagingAsync(Pagination pagination, ISpecification<T> spec = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
        {
            var guardExpression = GuardDataAccess(spec, ignoreOwner).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .BuildOrderBy(pagination.Sorts);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<T>(data, count);
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

        public virtual async Task<T> GetByIdAsync(long id, string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
        {
            if (includes != null && includes.Any())
            {
                return await FindBySpecificationAsync(new GetByIdSpecification<T>(id), includes, ignoreOwner, ignoreBranch, cancellationToken);
            }

            var key = GetCacheKey(id);
            var data = await _redisCache.GetAsync<T>(key, cancellationToken);
            if (data == null)
            {
                data = await FindBySpecificationAsync(new GetByIdSpecification<T>(id), includes, ignoreOwner, ignoreBranch, cancellationToken);
                if (data != null)
                {
                    await _redisCache.SetAsync(key, data, TimeSpan.FromSeconds(AppCacheTime.RecordWithId), cancellationToken: cancellationToken);
                }
            }

            if (data == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            return data;
        }

        public virtual async Task<List<T>> GetByIdsAsync(IList<long> ids, string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
            => await GetBySpecificationAsync(new GetByIdsSpecification<T>(ids), includes, ignoreOwner, ignoreBranch, cancellationToken);

        public virtual async Task<List<T>> GetAsync(string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
        {
            if (includes != null && includes.Any())
            {
                return await GetBySpecificationAsync<T>(null, includes, ignoreOwner, ignoreBranch, cancellationToken);
            }

            var key = GetCacheKey(type: "all");
            var data = await _redisCache.GetAsync<List<T>>(key, cancellationToken: cancellationToken);
            if (data == null)
            {
                data = await GetBySpecificationAsync<T>(null, includes, ignoreOwner, ignoreBranch, cancellationToken);
                if (data != null && data.Any())
                {
                    await _redisCache.SetAsync(key, data, TimeSpan.FromSeconds(AppCacheTime.Records), cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(predicate, cancellationToken);
        }

        protected string GetCacheKey(long id = 0, string type = "")
        {
            if (type == "all")
            {
                if (typeof(T).HasInterface<IPersonalizeEntity>())
                {
                    return BaseCacheKeys.DbOwnerAllKey<T>(_executionContext.UserId);
                }
                return BaseCacheKeys.DbSystemAllKey<T>();
            }

            if (typeof(T).HasInterface<IPersonalizeEntity>())
            {
                return BaseCacheKeys.DbOwnerIdKey<T>(id, _executionContext.UserId);
            }
            return BaseCacheKeys.DbSystemIdKey<T>(id);
        }
    }
}
