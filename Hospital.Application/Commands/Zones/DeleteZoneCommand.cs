using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Zones
{
    [RequiredPermission(ActionExponent.ZoneManagement)]
    public class DeleteZoneCommand : BaseCommand
    {
        public DeleteZoneCommand(List<long> ids)
        {
            Ids = ids;
        }
        public List<long> Ids { get; }
    }
}
