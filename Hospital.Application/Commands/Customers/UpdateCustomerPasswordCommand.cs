using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{ 
    [RequiredPermission(ActionExponent.UpdateCustomer)]
    public class UpdateCustomerPasswordCommand : BaseCommand
    {
        public UpdateCustomerPasswordCommand(UserPwdModel model)
        {
            Model = model;
        }

        public UserPwdModel Model { get; }
    }
}
