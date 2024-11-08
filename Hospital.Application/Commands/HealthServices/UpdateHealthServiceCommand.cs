using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthServices
{
    public class UpdateHealthServiceCommand : BaseCommand
    {
        public UpdateHealthServiceCommand(HealthServiceDto healthService) 
        {
            HealthService = healthService;
        }
        public HealthServiceDto HealthService { get;  }
    }
}
