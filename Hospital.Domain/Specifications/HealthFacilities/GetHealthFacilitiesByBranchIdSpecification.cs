using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthFacilities
{
    public class GetHealthFacilitiesByBranchIdSpecification : ExpressionSpecification<HealthFacility>
    {
        public GetHealthFacilitiesByBranchIdSpecification(long branchId) : base(x => (x.BranchId == branchId))
        {
        }
    }
}
