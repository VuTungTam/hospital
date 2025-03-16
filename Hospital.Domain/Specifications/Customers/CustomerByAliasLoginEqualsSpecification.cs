using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Customers
{
    public class CustomerByAliasLoginEqualsSpecification : ExpressionSpecification<Customer>
    {
        public CustomerByAliasLoginEqualsSpecification(string alias) : base(x => x.AliasLogin == alias)
        {
        }
    }
}
