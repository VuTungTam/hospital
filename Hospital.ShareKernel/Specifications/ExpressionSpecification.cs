using System.Linq.Expressions;

namespace Hospital.SharedKernel.Specifications
{
    public class ExpressionSpecification<T> : Specification<T> where T : class
    {
        private Expression<Func<T, bool>> Expression { get; }

        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }
        public ExpressionSpecification()
        {
            
        }
        public override Expression<Func<T, bool>> GetExpression() => Expression;
    }
}
