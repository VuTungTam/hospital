using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByDateSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByDateSpecification(DateTime date) : base(x => x.Date == date)
        {
        }
    }
}
