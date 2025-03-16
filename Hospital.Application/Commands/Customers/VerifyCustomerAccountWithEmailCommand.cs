using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Models;
using Hospital.SharedKernel.Application.Services.Auth.Models;

namespace Hospital.Application.Commands.Customers
{
    public class VerifyCustomerAccountWithEmailCommand : BaseAllowAnonymousCommand<LoginResult>
    {
        public VerifyCustomerAccountWithEmailCommand(VerificationAccountModel model)
        {
            Model = model;
        }

        public VerificationAccountModel Model { get; }
    }
}
