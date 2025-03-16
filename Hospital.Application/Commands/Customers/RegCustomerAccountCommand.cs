using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(new ActionExponent[] { ActionExponent.AllowAnonymous })]
    public class RegCustomerAccountCommand : BaseCommand<string>
    {
        public RegCustomerAccountCommand(RegAccountRequest account)
        {
            Account = account;
        }
        public RegAccountRequest Account { get; }
    }
}
