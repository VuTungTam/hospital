using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityNamePaginationQuery : BaseAllowAnonymousQuery<PaginationResult<FacilityNameDto>>
    {
        public GetFacilityNamePaginationQuery(Pagination pagination)
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; }
    }
}
