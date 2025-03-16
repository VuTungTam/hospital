using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    public class SendVerifyCustomerAccountWithEmailCommand : BaseCommand
    {
        public SendVerifyCustomerAccountWithEmailCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }

}
