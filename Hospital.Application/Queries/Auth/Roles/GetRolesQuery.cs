using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Auth.Roles
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetRolesQuery : BaseQuery<List<RoleDto>>
    {
    }
}
