using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Specialties
{
    public class RemoveSpecialtyForFacilityCommand : BaseCommand
    {
        public RemoveSpecialtyForFacilityCommand(long facilityId, long specialtyId)
        {
            FacilityId = facilityId;
            SpecialtyId = specialtyId;
        }
        public long FacilityId { get; }
        public long SpecialtyId { get; }
    }
}
