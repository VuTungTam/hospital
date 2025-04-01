using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthFacilities
{
    [RequiredPermission(ActionExponent.UpdateFacility)]
    public class UpdateHealthFacilityCommand : BaseCommand
    {
        public UpdateHealthFacilityCommand(HealthFacilityDto healthFacility) 
        {
            HealthFacility = healthFacility;
        }
        public HealthFacilityDto HealthFacility { get; }
    }
}
