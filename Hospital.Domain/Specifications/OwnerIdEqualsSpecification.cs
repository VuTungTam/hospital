using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class OwnerIdEqualsSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public OwnerIdEqualsSpecification(long ownerId) : base(x => (x as IOwnedEntity).OwnerId == ownerId)
        {
        }
    }
}
