using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.Master)]
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
