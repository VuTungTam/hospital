using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByStatusEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByStatusEqualsSpecification(AccountStatus status) : base(x => x.Status == status)
        {
        }
    }
}
