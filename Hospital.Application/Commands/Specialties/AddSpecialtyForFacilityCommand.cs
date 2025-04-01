using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Specialties
{
    [RequiredPermission(ActionExponent.UpdateFacility)]
    public class AddSpecialtyForFacilityCommand : BaseCommand
    {
        public AddSpecialtyForFacilityCommand(long facilityId, long specialtyId)
        {
            FacilityId = facilityId;
            SpecialtyId = specialtyId;
        }
        public long FacilityId { get; }
        public long SpecialtyId { get; }
    }
}
