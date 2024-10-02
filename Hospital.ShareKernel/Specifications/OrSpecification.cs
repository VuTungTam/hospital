using System.Linq.Expressions;
using Hospital.SharedKernel.Specifications.Interfaces;

namespace Hospital.SharedKernel.Specifications
{
    public class OrSpecification<T> : Specification<T> where T : class
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var leftExpr = _left.GetExpression();
            var rightExpr = _right.GetExpression();

            var param = Expression.Parameter(typeof(T), "x");
            var orExpression = Expression.OrElse(
                Expression.Invoke(leftExpr, param),
                Expression.Invoke(rightExpr, param)
            );

            return Expression.Lambda<Func<T, bool>>(orExpression, param);
        }
    }
}
