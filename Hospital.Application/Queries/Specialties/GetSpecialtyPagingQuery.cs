using Hospital.Application.Dtos.Specialties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Validators;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyPagingQuery : BaseAllowAnonymousQuery<PaginationResult<SpecialtyDto>>
    {
        public GetSpecialtyPagingQuery(Pagination pagination, long facilityId)
        {
            Pagination = pagination;
            FacilityId = facilityId;
        }
        public Pagination Pagination { get; set; }

        public long FacilityId { get; }
    }
}
