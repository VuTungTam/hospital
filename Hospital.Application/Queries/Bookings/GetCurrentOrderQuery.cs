using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class GetCurrentOrderQuery : BaseAllowAnonymousQuery<int>
    {
        public GetCurrentOrderQuery(long serviceId) 
        {
            ServiceId = serviceId;
        }

        public long ServiceId { get; }
    }
}
