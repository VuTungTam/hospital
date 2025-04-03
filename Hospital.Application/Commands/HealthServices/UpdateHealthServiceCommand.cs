using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthServices
{
    [RequiredPermission(ActionExponent.UpdateService)]
    public class UpdateHealthServiceCommand : BaseCommand
    {
        public UpdateHealthServiceCommand(HealthServiceDto healthService)
        {
            HealthService = healthService;
        }
        public HealthServiceDto HealthService { get; }
    }
}
