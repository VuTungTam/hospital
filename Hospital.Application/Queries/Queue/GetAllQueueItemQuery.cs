using Hospital.Application.Dtos.Queue;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Queue
{
    public class GetAllQueueItemQuery : BaseQuery<List<QueueItemDto>>
    {
        public GetAllQueueItemQuery(DateTime created)
        {

            Created = created;

        }
        public DateTime Created { get;}
    }
}
