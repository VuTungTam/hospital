using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class BookingByDateLessThanOrEqualsSpecification : ExpressionSpecification<Booking>
    {
        public BookingByDateLessThanOrEqualsSpecification(DateTime date) : base(x => x.Date <= date)
        {
        }
    }
}

