using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class DeleteHealthFacilityCommand : BaseCommand
    {
        public DeleteHealthFacilityCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get; }
    }
}
