using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class GetCurrentOrderQuery : BaseAllowAnonymousQuery<int>
    {
        public GetCurrentOrderQuery(long serviceId, long timeSlotId) 
        {
            ServiceId = serviceId;
            TimeSlotId = timeSlotId;
        }

        public long ServiceId { get; }

        public long TimeSlotId { get; }
    }
}
