using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Customers
{
    public class GetCustomerProfileQuery : BaseQuery<CustomerDto>
    {
    }
}
