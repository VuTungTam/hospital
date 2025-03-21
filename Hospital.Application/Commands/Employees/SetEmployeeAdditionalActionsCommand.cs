using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.Master)]
    public class SetEmployeeAdditionalActionsCommand : BaseCommand
    {
        public SetEmployeeAdditionalActionsCommand(long employeeId, List<AdditionalAction> actions)
        {
            EmployeeId = employeeId;
            Actions = actions;
        }

        public long EmployeeId { get; }

        public List<AdditionalAction> Actions { get; }
    }
}
