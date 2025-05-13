using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingByQueueStatusSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingByQueueStatusSpecification() : base(x => (x.Status == BookingStatus.Doing || x.Status == BookingStatus.Confirmed || x.Status == BookingStatus.Completed))
        {
        }
    }
}
