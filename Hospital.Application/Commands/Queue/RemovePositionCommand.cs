using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class RemovePositionCommand : BaseCommand
    {
        public RemovePositionCommand(int position, DateTime created) 
        {
            Position = position;
            Created = created;
        }
        public int Position { get; }
        public DateTime Created {  get;}

    }
}
