using Hospital.SharedKernel.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.SharedKernel.Specifications
{
    public class AndSpecification<T> : Specification<T> where T : class
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var leftExpr = _left.GetExpression();
            var rightExpr = _right.GetExpression();

            var param = Expression.Parameter(typeof(T), "x");
            var andExpression = Expression.AndAlso(
                Expression.Invoke(leftExpr, param),
                Expression.Invoke(rightExpr, param)
            );

            return Expression.Lambda<Func<T, bool>>(andExpression, param);
        }
    }
}
