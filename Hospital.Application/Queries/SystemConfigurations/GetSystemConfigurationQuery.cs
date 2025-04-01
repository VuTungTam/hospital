using Hospital.Application.Dtos.SystemConfigurations;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.SystemConfigurations
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetSystemConfigurationQuery : BaseQuery<SystemConfigurationDto>
    {
    }
}
