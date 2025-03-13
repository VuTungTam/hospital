using Hospital.SharedKernel.Application.CQRS.Queries.Base;
namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingCountQuery : BaseAllowAnonymousQuery<int>
    {
        public GetBookingCountQuery(long? serviceId, DateTime? date, TimeSpan? serviceStartTime, TimeSpan? serviceEndTime) { 
            ServiceId = serviceId;
            Date = date;
            ServiceStartTime = serviceStartTime;
            ServiceEndTime = serviceEndTime;
        }
        public long? ServiceId { get; }
        public DateTime? Date { get; }
        public TimeSpan? ServiceStartTime { get; }
        public TimeSpan? ServiceEndTime { get; }
    }
}
