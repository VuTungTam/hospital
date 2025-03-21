using Hospital.Domain.Entities.Feedbacks;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Feedbacks
{
    public class GetFeedbackByServiceIdSpecification : ExpressionSpecification<Feedback>
    {
        public GetFeedbackByServiceIdSpecification(long serviceId) : base(x => x.Booking.ServiceId == serviceId)
        {
        }
    }
}
