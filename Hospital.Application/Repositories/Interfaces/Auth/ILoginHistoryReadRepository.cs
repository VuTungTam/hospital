using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Auths;

namespace Hospital.Application.Repositories.Interfaces.Auth
{
    public interface ILoginHistoryReadRepository : IReadRepository<LoginHistory>
    {
        Task<PaginationResult<LoginHistory>> GetPaginationWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default);
    }
}
