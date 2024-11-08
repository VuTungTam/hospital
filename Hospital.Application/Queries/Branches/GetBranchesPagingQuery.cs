using Hospital.Application.Dtos.Branches;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Branches
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetBranchesPagingQuery : BaseQuery<PagingResult<BranchDto>>
    {
        public GetBranchesPagingQuery(Pagination pagination, BranchStatus status)
        {
            Pagination = pagination;
            Status = status;
        }

        public Pagination Pagination { get; }
        public BranchStatus Status { get; }
    }
}
