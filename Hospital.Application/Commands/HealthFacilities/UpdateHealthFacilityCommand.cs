using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class UpdateHealthFacilityCommand : BaseCommand
    {
        public UpdateHealthFacilityCommand(HealthFacilityDto healthFacility) 
        {
            HealthFacility = healthFacility;
        }
        public HealthFacilityDto HealthFacility { get; }
    }
}
