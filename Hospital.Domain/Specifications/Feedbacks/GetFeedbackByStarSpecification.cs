using Hospital.Domain.Entities.Feedbacks;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Feedbacks
{
    public class GetFeedbackByStarSpecification : ExpressionSpecification<Feedback>
    {
        public GetFeedbackByStarSpecification(int star) : base(x => x.Stars == star)
        {
        }
    }
}
