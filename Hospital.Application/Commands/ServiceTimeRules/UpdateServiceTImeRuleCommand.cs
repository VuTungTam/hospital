using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class UpdateServiceTImeRuleCommand : BaseCommand
    {
        public UpdateServiceTImeRuleCommand(ServiceTimeRuleDto dto)
        {
            Dto = dto;
        }
        public ServiceTimeRuleDto Dto { get; }
    }
}
