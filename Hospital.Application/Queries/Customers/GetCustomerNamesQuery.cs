using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Customers
{
    [RequiredPermission(ActionExponent.ViewBooking)]
    public class GetCustomerNamesQuery : BaseQuery<List<CustomerNameDto>>
    {
    }
}
