using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Models;

namespace Hospital.Application.Commands.Accounts.Passwords
{
    public class ForgotPasswordStep2Command : BaseAllowAnonymousCommand<string>
    {
        public ForgotPasswordStep2Command(VerifyPwdCodeModel model)
        {
            Model = model;
        }

        public VerifyPwdCodeModel Model { get; }
    }
}
