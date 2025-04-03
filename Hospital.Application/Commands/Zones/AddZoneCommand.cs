using Hospital.Application.Dtos.Zones;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Zones
{
    [RequiredPermission(ActionExponent.ZoneManagement)]
    public class AddZoneCommand : BaseCommand<string>
    {
        public AddZoneCommand(ZoneDto zone)
        {
            Zone = zone;
        }
        public ZoneDto Zone { get; set; }
    }
}
