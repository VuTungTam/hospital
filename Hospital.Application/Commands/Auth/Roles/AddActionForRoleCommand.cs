using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Auth.Roles
{
    [RequiredPermission(ActionExponent.Master)]
    public class AddActionForRoleCommand : BaseCommand
    {
        public AddActionForRoleCommand(long roleId, long actionId)
        {
            RoleId = roleId;
            ActionId = actionId;
        }

        public long RoleId { get; }
        public long ActionId { get; }
    }
}
