using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetFeedbackByBookingIdQuery : BaseAllowAnonymousQuery<FeedbackDto>
    {
        public GetFeedbackByBookingIdQuery(long bookingId)
        {
            BookingId = bookingId;
        }
        public long BookingId { get; }
    }
}
