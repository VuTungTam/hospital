using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Queue
{
    public class GetCurrentQueueItemQuery : BaseQuery<int>
    {
        public GetCurrentQueueItemQuery(long serviceId)
        {
            ServiceId = serviceId;
        }
        public long ServiceId { get; }
    }
}
