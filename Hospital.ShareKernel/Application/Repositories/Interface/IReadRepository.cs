using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Specifications.Interfaces;
using System.Linq.Expressions;

namespace Hospital.SharedKernel.Application.Repositories.Interface
{
    public interface IReadRepository<T> : IOrmRepository where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id, string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default);

        Task<List<T>> GetByIdsAsync(IList<long> id, string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default);

        Task<List<T>> GetAsync(string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default);

        Task<PagingResult<T>> GetPagingAsync(Pagination pagination, ISpecification<T> spec = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);

    }
}
