using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthProfiles
{
    [RequiredPermission(ActionExponent.AddProfile)]
    public class AddHealthProfileCommand : BaseCommand<string>
    {
        public AddHealthProfileCommand(HealthProfileDto dto)
        {
            Dto = dto;
        }
        public HealthProfileDto Dto { get; set; }
    }
}
