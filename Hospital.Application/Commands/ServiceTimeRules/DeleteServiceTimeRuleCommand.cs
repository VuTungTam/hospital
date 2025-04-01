using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    [RequiredPermission(ActionExponent.UpdateService)]
    public class DeleteServiceTimeRuleCommand : BaseCommand
    {
        public DeleteServiceTimeRuleCommand(List<long> ids) 
        { 
            Ids = ids;
        }
        public List<long> Ids { get; set; }
        
    }
}
