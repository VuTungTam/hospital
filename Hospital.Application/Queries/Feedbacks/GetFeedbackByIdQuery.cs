using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetFeedbackByIdQuery : BaseAllowAnonymousQuery<FeedbackDto>
    {
        public GetFeedbackByIdQuery(long id) 
        { 
            Id = id;
        }
        public long Id { get; }
    }
}
