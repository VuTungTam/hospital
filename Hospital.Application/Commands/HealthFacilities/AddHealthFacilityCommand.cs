using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class AddHealthFacilityCommand : BaseCommand<string>
    {
        public AddHealthFacilityCommand(HealthFacilityDto healthFacility)
        {
            HealthFacility = healthFacility;
        }
        public HealthFacilityDto HealthFacility { get; set; }
    }
}
