using Hospital.Application.Dtos.Branches;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Branches
{
    public class GetBranchesQuery : BaseAllowAnonymousQuery<List<BranchDto>>
    {
    }
}
