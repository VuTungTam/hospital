using Hospital.Application.Dtos.Payments;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Payments
{
    public class GetPaymentValidByBookingIdQuery : BaseQuery<PaymentDto>
    {
        public GetPaymentValidByBookingIdQuery(long bookingId)
        {
            BookingId = bookingId;
        }

        public long BookingId { get; }
    }
}
