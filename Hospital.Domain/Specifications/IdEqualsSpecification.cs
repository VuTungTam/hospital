using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class IdEqualsSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public IdEqualsSpecification(long id) : base(x => x.Id == id)
        {
        }
    }
}
