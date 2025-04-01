using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthServices
{
    [RequiredPermission(ActionExponent.AddService)]
    public class AddHealthServiceCommand : BaseCommand<string>
    {
        public AddHealthServiceCommand(HealthServiceDto healthService)
        {
            HealthService = healthService;
        }
        public HealthServiceDto HealthService { get; set; }
    }
}
