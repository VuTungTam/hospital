using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class GetByIdsSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public GetByIdsSpecification(IList<long> ids) : base(x => ids.Contains(x.Id))
        {
        }
    }
}
