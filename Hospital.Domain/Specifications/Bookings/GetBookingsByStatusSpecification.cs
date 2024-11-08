using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByStatusSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByStatusSpecification(BookingStatus status) : base(x => (status == BookingStatus.None || x.Status == status))
        {
        }
    }
}
