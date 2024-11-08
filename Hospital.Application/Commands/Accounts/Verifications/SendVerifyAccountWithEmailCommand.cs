using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Accounts.Verifications
{
    public class SendVerifyAccountWithEmailCommand : BaseCommand
    {
        public SendVerifyAccountWithEmailCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
   
}
