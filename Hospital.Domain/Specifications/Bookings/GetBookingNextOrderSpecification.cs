using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingNextOrderSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingNextOrderSpecification(int order) : base(x => x.Order > order)
        {
        }
    }
}
