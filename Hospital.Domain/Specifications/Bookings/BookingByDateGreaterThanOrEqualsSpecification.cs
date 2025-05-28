using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;
namespace VetHospital.Domain.Specifications.Bookings
{
    public class BookingByDateGreaterThanOrEqualsSpecification : ExpressionSpecification<Booking>
    {
        public BookingByDateGreaterThanOrEqualsSpecification(DateTime date) : base(x => x.CreatedAt >= date)
        {
        }
    }
}
