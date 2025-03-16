using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByZaloIdEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByZaloIdEqualsSpecification(string zaloId) : base(x => x.ZaloId == zaloId)
        {
        }
    }
}
