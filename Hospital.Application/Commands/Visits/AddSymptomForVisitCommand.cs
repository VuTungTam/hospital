using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Visits
{
    public class AddSymptomForVisitCommand : BaseCommand
    {
        public AddSymptomForVisitCommand(long visitId, List<long> symptomIds)
        {
            VisitId = visitId;
            SymptomIds = symptomIds;
        }
        public long VisitId { get; }
        public List<long> SymptomIds { get; }
    }
}
