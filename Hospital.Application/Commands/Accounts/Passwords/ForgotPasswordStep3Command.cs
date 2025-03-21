using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Accounts.Passwords
{
    public class ForgotPasswordStep3Command : BaseAllowAnonymousCommand
    {
        public ForgotPasswordStep3Command(ChangePasswordRequest dto)
        {
            Dto = dto;
        }

        public ChangePasswordRequest Dto { get; }
    }
}
