using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthProfiles
{
    public class AddHealthProfileCommand : BaseAllowAnonymousCommand<long>
    {
        public AddHealthProfileCommand(HealthProfileDto dto)
        {
            Dto = dto;
            
        }
        public HealthProfileDto Dto { get; set; }
    }
}
