using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByEmailEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByEmailEqualsSpecification(string email) : base(x => x.Email == email)
        {
        }
    }
}
