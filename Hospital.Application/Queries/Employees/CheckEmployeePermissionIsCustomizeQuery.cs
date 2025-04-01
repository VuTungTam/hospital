using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Employees
{
    [RequiredPermission(ActionExponent.ViewEmployee)]
    public class CheckEmployeePermissionIsCustomizeQuery : BaseQuery<bool>
    {
        public CheckEmployeePermissionIsCustomizeQuery(long employeeId)
        {
            EmployeeId = employeeId;
        }

        public long EmployeeId { get; }
    }
}
