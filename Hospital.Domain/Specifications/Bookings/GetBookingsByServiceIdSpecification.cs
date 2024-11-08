using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByServiceIdSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByServiceIdSpecification(long serviceId) : base(x => x.ServiceId == serviceId)
        {
        }
    }
}
