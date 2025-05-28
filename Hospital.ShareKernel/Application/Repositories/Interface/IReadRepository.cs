using System.Linq.Expressions;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Specifications.Interfaces;

namespace Hospital.SharedKernel.Application.Repositories.Interface
{
    public interface IReadRepository<T> : IOrmRepository where T : BaseEntity
    {
        QueryOption DefaultQueryOption { get; }

        Task<T> GetByIdAsync(long id, QueryOption option = default, CancellationToken cancellationToken = default);

        Task<List<T>> GetByIdsAsync(IList<long> id, QueryOption option = default, CancellationToken cancellationToken = default);

        Task<List<T>> GetAsync(QueryOption option = default, CancellationToken cancellationToken = default);

        Task<PaginationResult<T>> GetPaginationAsync(Pagination pagination, ISpecification<T> spec, QueryOption option = default, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);
    }
}
