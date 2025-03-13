using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByTimeRangeSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByTimeRangeSpecification(TimeSpan ServiceStartTime, TimeSpan ServiceEndTime) : 
            base(x => x.ServiceStartTime == ServiceStartTime && x.ServiceEndTime == ServiceEndTime)
        {
        }
    }
}
