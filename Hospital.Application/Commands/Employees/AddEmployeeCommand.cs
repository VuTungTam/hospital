using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.AddEmployee)]
    public class AddEmployeeCommand : BaseCommand<string>
    {
        public AddEmployeeCommand(EmployeeDto employee)
        {
            Employee = employee;
        }

        public EmployeeDto Employee { get; }
    }
}
