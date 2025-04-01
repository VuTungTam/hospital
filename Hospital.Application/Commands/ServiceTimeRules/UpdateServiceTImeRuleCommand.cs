using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    [RequiredPermission(ActionExponent.UpdateService)]
    public class UpdateServiceTimeRuleCommand : BaseCommand
    {
        public UpdateServiceTimeRuleCommand(ServiceTimeRuleDto dto)
        {
            Dto = dto;
        }
        public ServiceTimeRuleDto Dto { get; }
    }
}
