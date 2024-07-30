using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludesRelateData<T>(this IQueryable<T> query, string[] includes) where T : class
        {
            if (includes != null && includes.Any())
            {
                return includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}
