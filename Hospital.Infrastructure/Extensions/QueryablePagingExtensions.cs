using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MassTransit.Internals;


namespace VetHospital.Infrastructure.Extensions
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

            if (typeof(T).HasInterface<IModified>() && typeof(T).HasInterface<ICreated>())
            {
                return query.OrderByDescending(x => (x as IModified).Modified ?? (x as ICreated).Created);
            }

            if (typeof(T).HasInterface<IModified>() && !typeof(T).HasInterface<ICreated>())
            {
                return query.OrderByDescending(x => (x as IModified).Modified);
            }

            if (typeof(T).HasInterface<ICreated>())
            {
                return query.OrderByDescending(x => (x as ICreated).Created);
            }

            return query;
        }

        public static IQueryable<T> BuildLimit<T>(this IQueryable<T> query, int skip, int take) where T : BaseEntity
        {
            return query.Skip(skip).Take(take);
        }
    }
}
