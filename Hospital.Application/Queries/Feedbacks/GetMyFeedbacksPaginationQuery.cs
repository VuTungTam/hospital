using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetMyFeedbacksPaginationQuery : BaseQuery<PaginationResult<FeedbackDto>>
    {
        public GetMyFeedbacksPaginationQuery(Pagination pagination, int star, long serviceId)
        {
            Pagination = pagination;
            Star = star;
            ServiceId = serviceId;
        }

        public Pagination Pagination { get; set; }

        public int Star { get; set; }

        public long ServiceId { get; set; }
    }
}
