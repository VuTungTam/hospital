using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.Services.Auth.Enums;

namespace Hospital.Application.Queries.Customers
{
    //[RequiredPermission(ActionExponent.ViewCustomer)]
    public class GetCustomerByIdQuery : BaseQuery<CustomerDto>
    {
        public GetCustomerByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
