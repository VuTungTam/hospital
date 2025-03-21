using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.DeleteEmployee)]
    public class DeleteEmployeesCommand : BaseCommand
    {
        public DeleteEmployeesCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
