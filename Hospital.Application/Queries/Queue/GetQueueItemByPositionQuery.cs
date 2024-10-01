using Hospital.Application.Dtos.Queue;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Queue
{
    public class GetQueueItemByPositionQuery : BaseQuery<QueueItemDto>
    {
        public GetQueueItemByPositionQuery(long serviceId,int position, DateTime created) 
        {
            Position = position;
            Created = created;
            ServiceId = serviceId;
        }
        public int Position { get;}
        public DateTime Created {  get; }
        public long ServiceId { get; }
    }
}
