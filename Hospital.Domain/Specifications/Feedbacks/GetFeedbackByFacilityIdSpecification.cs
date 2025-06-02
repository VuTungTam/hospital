using Hospital.Domain.Entities.Feedbacks;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Feedbacks
{
    public class GetFeedbackByFacilityIdSpecification : ExpressionSpecification<Feedback>
    {
        public GetFeedbackByFacilityIdSpecification(long facilityId) : base(x => x.FacilityId == facilityId)
        {
        }
    }
}
