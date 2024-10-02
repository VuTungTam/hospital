using Hospital.Application.Dtos.Declarations;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class AddVisitToQueueCommand : BaseCommand<int>
    {
        public AddVisitToQueueCommand(long visitId) {
            VisitId = visitId;
        }
        public long VisitId { get; set; }
    }
}
