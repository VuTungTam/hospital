using Hospital.SharedKernel.Specifications.Interfaces;
using System.Linq.Expressions;

namespace Hospital.SharedKernel.Specifications
{
    public abstract class Specification<T> : ISpecification<T> where T : class
    {
        public abstract Expression<Func<T, bool>> GetExpression();

        public bool IsSatisfiedBy(T o)
        {
            var predicate = GetExpression().Compile();
            return predicate(o);
        }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public ISpecification<T> Not(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, new NotSpecification<T>(specification));
        }
    }
}
