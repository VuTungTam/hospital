using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    public class ChangeEmployeePasswordCommand : BaseCommand
    {
        public ChangeEmployeePasswordCommand(ChangePasswordRequest dto)
        {
            Dto = dto;
        }

        public ChangePasswordRequest Dto { get; }
    }
}
