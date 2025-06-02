using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetFeedbacksPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<FeedbackDto>>
    {
        public GetFeedbacksPaginationQuery(Pagination pagination, int star, long serviceId, long facilityId)
        {
            Pagination = pagination;
            Star = star;
            ServiceId = serviceId;
            FacilityId = facilityId;
        }

        public Pagination Pagination { get; set; }

        public int Star { get; set; }

        public long ServiceId { get; set; }

        public long FacilityId { get; set; }
    }
}
