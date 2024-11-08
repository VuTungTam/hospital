using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class AddServiceTimeRuleCommand : BaseCommand<string>
    {
        public AddServiceTimeRuleCommand(ServiceTimeRuleDto timeRule)
        {
            TimeRule = timeRule;
        }
        public ServiceTimeRuleDto TimeRule { get;  }
    }
}
