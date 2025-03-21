using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    public class UpdateCustomerPasswordCommand : BaseCommand
    {
        public UpdateCustomerPasswordCommand(UserPwdModel model)
        {
            Model = model;
        }

        public UserPwdModel Model { get; }
    }
}
