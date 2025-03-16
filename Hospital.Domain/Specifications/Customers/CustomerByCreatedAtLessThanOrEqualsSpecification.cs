using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByCreatedAtLessThanOrEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByCreatedAtLessThanOrEqualsSpecification(DateTime date) : base(x => x.CreatedAt <= date)
        {
        }
    }
}
