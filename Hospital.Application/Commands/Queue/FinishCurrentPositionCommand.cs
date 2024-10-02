using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class FinishCurrentPositionCommand : BaseCommand
    {
        public FinishCurrentPositionCommand(long serviceId) {
            ServiceId = serviceId;
        }
        public long ServiceId { get; set; }
    }
}
