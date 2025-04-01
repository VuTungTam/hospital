using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthServices
{
    [RequiredPermission(ActionExponent.DeleteService)]
    public class DeleteHealthServiceCommand : BaseCommand
    {
        public DeleteHealthServiceCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get; }
    }
}
