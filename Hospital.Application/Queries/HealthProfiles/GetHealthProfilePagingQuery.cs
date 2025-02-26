using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetHealthProfilePagingQuery : BaseQuery<PagingResult<HealthProfileDto>>
    {
        public GetHealthProfilePagingQuery(Pagination pagination, HealthFacilityStatus status)
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; }
    }
}
