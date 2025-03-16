using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByCreatedAtGreaterThanOrEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByCreatedAtGreaterThanOrEqualsSpecification(DateTime date) : base(x => x.CreatedAt >= date)
        {
        }
    }
}
