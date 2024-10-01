using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Branches;

namespace Hospital.Application.Repositories.Interfaces.Branches
{
    public interface IBranchReadRepository : IReadRepository<Branch>
    {
        Task<PagingResult<Branch>> GetPagingWithFilterAsync(Pagination pagination, BranchStatus status, CancellationToken cancellationToken);
    }
}
