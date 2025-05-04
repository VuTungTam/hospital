using Hospital.Domain.Entities.Payments;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Payments
{
    public class GetPaymentsByBookingIdSpecification : ExpressionSpecification<Payment>
    {
        public GetPaymentsByBookingIdSpecification(long bookingId) : base(x => (x.BookingId == bookingId))
        {
        }
    }
}
