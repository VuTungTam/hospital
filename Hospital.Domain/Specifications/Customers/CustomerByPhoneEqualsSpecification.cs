using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByPhoneEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByPhoneEqualsSpecification(string phone) : base(x => x.Phone == phone)
        {
        }
    }
}

