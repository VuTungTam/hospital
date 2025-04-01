using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthProfiles
{
    [RequiredPermission(ActionExponent.DeleteProfile)]
    public class DeleteHealthProfileCommand : BaseCommand
    {
        public DeleteHealthProfileCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
