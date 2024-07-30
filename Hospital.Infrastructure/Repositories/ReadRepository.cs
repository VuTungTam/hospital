using Hospital.Domain.Constants;
using Hospital.Domain.Specifications;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Repositories.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infra.Repositories
{
    public class ReadRepository<T> : OrmRepository, IReadRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IStringLocalizer<Resources> _localizer;
        public ReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer
        ) : base(serviceProvider)
        {
            _dbSet = _dbContext.Set<T>();
            _localizer = localizer;
        }

        public virtual async Task<List<T>> GetAsync(string[] includes = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetByIdAsync(long id, string[] includes = null, CancellationToken cancellationToken = default)
        {
            return await FindBySpecificationAsync(new GetByIdSpecification<T>(id), new QueryOption(includes), cancellationToken);
        }

        public virtual async Task<List<T>> GetByIdsAsync(IList<long> ids, string[] includes = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<PagingResult<T>> GetPagingAsync(Pagination pagination, ISpecification<T> spec = null, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
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
    }
}
