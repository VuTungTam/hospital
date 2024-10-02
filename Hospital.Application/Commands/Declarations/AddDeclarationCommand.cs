using Hospital.Application.Dtos.Declarations;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Declarations
{
    public class AddDeclarationCommand : BaseCommand<long>
    {
        public AddDeclarationCommand(DeclarationDto dto)
        {
            Dto = dto;
            
        }
        public DeclarationDto Dto { get; set; }
    }
}
