using System.Linq.Expressions;

namespace Hospital.SharedKernel.Specifications.Interfaces
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> GetExpression();

        bool IsSatisfiedBy(T o);

        ISpecification<T> And(ISpecification<T> specification);

        ISpecification<T> Or(ISpecification<T> specification);

        ISpecification<T> Not(ISpecification<T> specification);
    }
}
