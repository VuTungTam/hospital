using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Models;
using Hospital.SharedKernel.Application.Services.Auth.Models;

namespace Hospital.Application.Commands.Accounts.Verifications
{
    public class VerifyAccountWithEmailCommand : BaseAllowAnonymousCommand<LoginResult>
    {
        public VerifyAccountWithEmailCommand(VerificationAccountModel model, string with)
        {
            Model = model;
            With = with;
        }

        public VerificationAccountModel Model { get; }
        public string With { get; }
    }
}
