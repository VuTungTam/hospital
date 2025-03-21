using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.UpdateEmployee)]
    public class UpdateEmployeeCommand : BaseCommand
    {
        public UpdateEmployeeCommand(EmployeeDto employee)
        {
            Employee = employee;
        }

        public EmployeeDto Employee { get; }
    }
}
