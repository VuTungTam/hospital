using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthProfiles
{
    public class UpdateHealthProfileCommand : BaseCommand
    {
        public UpdateHealthProfileCommand(HealthProfileDto healthProfile) 
        {
            HealthProfile = healthProfile;
        }

        public HealthProfileDto HealthProfile { get; set; }
    }
}
