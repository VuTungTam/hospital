using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetServiceTimeRulePagingQuery : BaseAllowAnonymousQuery<PaginationResult<ServiceTimeRuleDto>>
    {
        public GetServiceTimeRulePagingQuery(Pagination pagination, long serviceId, int dayOfWeek)
        {
            Pagination = pagination;
            ServiceId = serviceId;
            DayOfWeek = dayOfWeek;
        }
        public long ServiceId { get; }
        public int DayOfWeek { get; }
        public Pagination Pagination { get; }
    }
}
