using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.Master)]
    public class SetEmployeeActionAsDefaultCommand : BaseCommand
    {
        public SetEmployeeActionAsDefaultCommand(long employeeId)
        {
            EmployeeId = employeeId;
        }

        public long EmployeeId { get; }
    }
}
