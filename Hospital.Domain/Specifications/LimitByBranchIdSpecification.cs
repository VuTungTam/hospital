using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class LimitByBranchIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public LimitByBranchIdSpecification(long branchId) : base(x => (x as IBranchId).BranchId == branchId)
        {
        }
    }
}
