using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class UpdateServiceTimeRuleCommand : BaseCommand
    {
        public UpdateServiceTimeRuleCommand(ServiceTimeRuleDto dto)
        {
            Dto = dto;
        }
        public ServiceTimeRuleDto Dto { get; }
    }
}
