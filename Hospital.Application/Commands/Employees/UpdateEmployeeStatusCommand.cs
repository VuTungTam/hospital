using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.UpdateEmployee)]
    public class UpdateEmployeeStatusCommand : BaseCommand
    {
        public UpdateEmployeeStatusCommand(long id, AccountStatus status)
        {
            Id = id;
            Status = status;
        }

        public long Id { get; }
        public AccountStatus Status { get; }
    }
}
