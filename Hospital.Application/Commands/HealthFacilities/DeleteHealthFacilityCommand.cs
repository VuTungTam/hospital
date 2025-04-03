using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.HealthFacilities
{
    [RequiredPermission(ActionExponent.DeleteFacility)]
    public class DeleteHealthFacilityCommand : BaseCommand
    {
        public DeleteHealthFacilityCommand(List<long> ids)
        {
            Ids = ids;
        }
        public List<long> Ids { get; }
    }
}
