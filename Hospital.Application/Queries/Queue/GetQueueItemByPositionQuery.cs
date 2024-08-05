using Hospital.Application.Dtos.Queue;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Queue
{
    public class GetQueueItemByPositionQuery : BaseQuery<QueueItemDto>
    {
        public GetQueueItemByPositionQuery(int position, DateTime created) 
        {
            Position = position;
            Created = created;
        }
        public int Position { get;}
        public DateTime Created {  get; }
    }
}
