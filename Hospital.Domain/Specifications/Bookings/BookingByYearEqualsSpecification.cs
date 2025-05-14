using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class BookingByYearEqualsSpecification : ExpressionSpecification<Booking>
    {
        public BookingByYearEqualsSpecification(int year) : base(x => x.Date.Year == year)
        {
        }
    }
}
