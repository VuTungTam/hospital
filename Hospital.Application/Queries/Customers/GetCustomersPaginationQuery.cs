using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Queries.Customers
{
    //[RequiredPermission(ActionExponent.ViewCustomer)]
    public class GetCustomersPaginationQuery : BaseQuery<PaginationResult<CustomerDto>>
    {
        public GetCustomersPaginationQuery(Pagination pagination, AccountStatus state)
        {
            Pagination = pagination;
            State = state;
        }

        public Pagination Pagination { get; }
        public AccountStatus State { get; }
    }
}
