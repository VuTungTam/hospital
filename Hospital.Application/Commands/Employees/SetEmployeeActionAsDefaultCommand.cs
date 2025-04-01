using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Employees
{
    [RequiredPermission(ActionExponent.ChangeRole)]
    public class SetEmployeeActionAsDefaultCommand : BaseCommand
    {
        public SetEmployeeActionAsDefaultCommand(long employeeId)
        {
            EmployeeId = employeeId;
        }

        public long EmployeeId { get; }
    }
}
