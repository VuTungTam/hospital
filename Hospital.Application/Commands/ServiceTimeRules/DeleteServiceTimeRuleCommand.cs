using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class DeleteServiceTimeRuleCommand : BaseCommand
    {
        public DeleteServiceTimeRuleCommand(List<long> ids) 
        { 
            Ids = ids;
        }
        public List<long> Ids { get; set; }
        
    }
}
