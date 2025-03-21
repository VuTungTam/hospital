using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    public class ChangeCustomerPasswordCommand : BaseCommand
    {
        public ChangeCustomerPasswordCommand(ChangePasswordRequest dto)
        {
            Dto = dto;
        }

        public ChangePasswordRequest Dto { get; }
    }
}
