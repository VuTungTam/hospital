using Hospital.Domain.Entities.CancelReasons;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.CancelReasons
{
    public class GetCancelReasonByBookingIdSpecification : ExpressionSpecification<CancelReason>
    {
        public GetCancelReasonByBookingIdSpecification(long bookingId) : base(x => x.BookingId == bookingId)
        {
        }
    }
}
