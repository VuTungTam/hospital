using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Accounts.Reg
{
    [RequiredPermission(new ActionExponent[] { ActionExponent.AllowAnonymous})]
    public class RegAccountCommand : BaseCommand<string>
    {
        public RegAccountCommand(RegAccountDto account)
        {
            Account = account;
        }
        public RegAccountDto Account { get; }
    }
}
