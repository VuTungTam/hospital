using Hospital.Application.Dtos.Branches;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Branches
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetBranchByIdQuery : BaseQuery<BranchDto>
    {
        public GetBranchByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
