using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByHealthProfileIdSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByHealthProfileIdSpecification(long healthProfileId) : base(x => x.HealthProfileId == healthProfileId)
        {
        }
    }
}
