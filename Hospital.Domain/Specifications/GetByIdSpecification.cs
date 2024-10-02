using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Specifications;
using System.Linq.Expressions;

namespace Hospital.Domain.Specifications
{
    public class GetByIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public GetByIdSpecification(long id) : base(x => x.Id == id)
        {
        }
    }
}
