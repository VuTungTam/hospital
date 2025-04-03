using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    [RequiredPermission(ActionExponent.UpdateService)]
    public class AddServiceTimeRuleCommand : BaseCommand<string>
    {
        public AddServiceTimeRuleCommand(ServiceTimeRuleDto timeRule)
        {
            TimeRule = timeRule;
        }
        public ServiceTimeRuleDto TimeRule { get; }
    }
}
