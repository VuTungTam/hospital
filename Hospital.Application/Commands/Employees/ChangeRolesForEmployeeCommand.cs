using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Employees
{
    [RequiredPermission(ActionExponent.ChangeRole)]
    public class ChangeRolesForEmployeeCommand : BaseCommand
    {
        public ChangeRolesForEmployeeCommand(long userId, List<long> roleIds)
        {
            UserId = userId;
            RoleIds = roleIds;
        }

        public long UserId { get; }
        public List<long> RoleIds { get; }
    }
}
