using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Employees
{
    //[RequiredPermission(ActionExponent.ViewEmployee)]
    public class GetEmployeeByIdIncludeActionsQuery : BaseQuery<EmployeeDto>
    {
        public GetEmployeeByIdIncludeActionsQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
