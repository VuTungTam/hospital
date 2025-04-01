using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Employees
{
    [RequiredPermission(ActionExponent.ChangeRole)]
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
