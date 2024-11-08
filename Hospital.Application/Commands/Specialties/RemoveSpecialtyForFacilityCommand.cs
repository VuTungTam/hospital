using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Specialties
{
    public class RemoveSpecialtyForFacilityCommand : BaseCommand
    {
        public RemoveSpecialtyForFacilityCommand(long branchId, long specialtyId)
        {
            BranchId = branchId;
            SpecialtyId = specialtyId;
        }
        public long BranchId { get; }
        public long SpecialtyId { get; }
    }
}
