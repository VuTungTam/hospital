using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Customers
{
    //[RequiredPermission(ActionExponent.ViewBooking)]
    public class GetCustomerNamesQuery : BaseQuery<List<CustomerNameDto>>
    {
    }
}
