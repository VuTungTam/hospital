using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByCodeEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByCodeEqualsSpecification(string code) : base(x => x.Code == code)
        {
        }
    }
}
