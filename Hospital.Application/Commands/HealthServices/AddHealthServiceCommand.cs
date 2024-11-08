using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthServices
{
    public class AddHealthServiceCommand : BaseCommand<string>
    {
        public AddHealthServiceCommand(HealthServiceDto healthService)
        {
            HealthService = healthService;
        }
        public HealthServiceDto HealthService { get; set; }
    }
}
