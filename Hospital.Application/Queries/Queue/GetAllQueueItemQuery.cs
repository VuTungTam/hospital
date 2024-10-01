using Hospital.Application.Dtos.Queue;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Queue
{
    public class GetAllQueueItemQuery : BaseQuery<List<QueueItemDto>>
    {
        public GetAllQueueItemQuery(long serviceId,DateTime created)
        {

            Created = created;
            ServiceId = serviceId;
        }
        public DateTime Created { get;}
        public long ServiceId { get;}
    }
}
