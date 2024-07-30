using Hospital.Application.Dtos.Declarations;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Declarations
{
    public class AddDeclarationCommand : BaseCommand<long>
    {
        public AddDeclarationCommand(DeclarationDto dto, List<long> symptomIds)
        {
            Dto = dto;
            SymptomIds = symptomIds;
        }
        public DeclarationDto Dto { get; set; }
        public List<long> SymptomIds { get; set; }
    }
}
