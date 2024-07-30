using Hospital.Application.Dtos.Declarations;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class AddDeclarationToQueueCommand : BaseCommand<int>
    {
        public AddDeclarationToQueueCommand(long declarationId) {
            DeclarationId = declarationId;
        }
        public long DeclarationId { get; set; }
    }
}
