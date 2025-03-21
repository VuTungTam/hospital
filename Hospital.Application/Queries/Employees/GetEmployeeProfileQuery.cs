using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeeProfileQuery : BaseQuery<EmployeeDto>
    {
        public GetEmployeeProfileQuery(bool includeRole)
        {
            IncludeRole = includeRole;
        }

        public bool IncludeRole { get; }
    }
}
