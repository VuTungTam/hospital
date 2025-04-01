using Hospital.Application.Dtos.SystemConfigurations;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.SystemConfigurations
{
    [RequiredPermission(ActionExponent.Master)]
    public class UpdateSystemConfigurationCommand : BaseCommand
    {
        public UpdateSystemConfigurationCommand(SystemConfigurationDto systemConfigurationDto)
        {
            SystemConfigurationDto = systemConfigurationDto;
        }

        public SystemConfigurationDto SystemConfigurationDto { get; }
    }
}
