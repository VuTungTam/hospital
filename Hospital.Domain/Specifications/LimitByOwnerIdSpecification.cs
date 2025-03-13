using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class LimitByOwnerIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public LimitByOwnerIdSpecification(long userId) : base(x => (x as IOwnedEntity).OwnerId == userId)
        {
        }
    }
}
