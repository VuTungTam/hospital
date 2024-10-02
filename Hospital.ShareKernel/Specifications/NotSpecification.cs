using Hospital.SharedKernel.Specifications.Interfaces;
using System.Linq.Expressions;

namespace Hospital.SharedKernel.Specifications
{
    public class NotSpecification<T> : Specification<T> where T : class
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(_specification.GetExpression().Body), _specification.GetExpression().Parameters.Single());
        }
    }
}
