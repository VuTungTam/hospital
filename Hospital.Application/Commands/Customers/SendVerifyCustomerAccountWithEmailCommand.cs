using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(ActionExponent.UpdateCustomer)]
    public class SendVerifyCustomerAccountWithEmailCommand : BaseAllowAnonymousCommand
    {
        public SendVerifyCustomerAccountWithEmailCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }

}
