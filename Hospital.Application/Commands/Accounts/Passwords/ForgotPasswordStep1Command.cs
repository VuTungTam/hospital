using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Accounts.Passwords
{
    public class ForgotPasswordStep1Command : BaseAllowAnonymousCommand
    {
        public ForgotPasswordStep1Command(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
