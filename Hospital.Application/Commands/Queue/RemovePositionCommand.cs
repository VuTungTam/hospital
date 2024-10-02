using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class RemovePositionCommand : BaseCommand
    {
        public RemovePositionCommand(int position, DateTime created, long serviceId) 
        {
            Position = position;
            Created = created;
            ServiceId = serviceId;
        }
        public int Position { get; }
        public DateTime Created {  get;}
        public long ServiceId {  get;}

    }
}
