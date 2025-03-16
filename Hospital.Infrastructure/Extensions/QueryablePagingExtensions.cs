using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MassTransit.Internals;


namespace Hospital.Infrastructure.Extensions
{
    public static class QueryablePagingExtensionss
    {
        public static IQueryable<T> BuildOrderBy<T>(this IQueryable<T> query, List<SortModel> sorts) where T : BaseEntity
        {
            if (sorts != null && sorts.Any())
            {
                foreach (var sort in sorts)
                {
                    var fieldName = sort.FieldName.FirstCharToUpper();
                    if (sort.SortAscending)
                    {
                        query = query.OrderBy(x => x[fieldName]);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x[fieldName]);
                    }
                }
                return query;
            }

            if (typeof(T).HasInterface<IModifiedAt>() && typeof(T).HasInterface<ICreatedAt>())
            {
                return query.OrderByDescending(x => (x as IModifiedAt).ModifiedAt ?? (x as ICreatedAt).CreatedAt);
            }

            if (typeof(T).HasInterface<IModifiedAt>() && !typeof(T).HasInterface<ICreatedAt>())
            {
                return query.OrderByDescending(x => (x as IModifiedAt).ModifiedAt);
            }

            if (typeof(T).HasInterface<ICreatedAt>())
            {
                return query.OrderByDescending(x => (x as ICreatedAt).CreatedAt);
            }

            return query;
        }

        public static IQueryable<T> BuildLimit<T>(this IQueryable<T> query, int skip, int take) where T : BaseEntity
        {
            return query.Skip(skip).Take(take);
        }
    }
}
