using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Branches
{
    public class GetBranchesByStatusSpecification : ExpressionSpecification<Branch>
    {
        public GetBranchesByStatusSpecification(BranchStatus status) : base(x => status == BranchStatus.None || x.Active == (status == BranchStatus.Active))
        {
        }
    }
}
