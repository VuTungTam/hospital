using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    //Sửa
    public class SendVerifyCustomerAccountWithEmailCommand : BaseAllowAnonymousCommand
    {
        public SendVerifyCustomerAccountWithEmailCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }

}
