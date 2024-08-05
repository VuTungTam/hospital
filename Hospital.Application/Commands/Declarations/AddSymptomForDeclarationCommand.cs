using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Declarations
{
    public class AddSymptomForDeclarationCommand : BaseCommand
    {
        public AddSymptomForDeclarationCommand(long declarationId, List<long> symptomIds)
        {
            DeclarationId = declarationId;
            SymptomIds = symptomIds;
        }
        public long DeclarationId { get; }
        public List<long> SymptomIds {  get; }
    }
}
