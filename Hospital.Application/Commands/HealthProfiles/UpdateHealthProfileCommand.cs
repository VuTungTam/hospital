using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthProfiles
{
    [RequiredPermission(ActionExponent.UpdateProfile)]
    public class UpdateHealthProfileCommand : BaseCommand
    {
        public UpdateHealthProfileCommand(HealthProfileDto healthProfile)
        {
            HealthProfile = healthProfile;
        }

        public HealthProfileDto HealthProfile { get; set; }
    }
}
