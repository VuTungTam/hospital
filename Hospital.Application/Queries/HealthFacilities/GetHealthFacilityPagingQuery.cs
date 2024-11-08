using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetHealthFacilityPagingQuery : BaseAllowAnonymousQuery<PagingResult<HealthFacilityDto>>
    {
        public GetHealthFacilityPagingQuery(Pagination pagination, long brandId, long typeId, HealthFacilityStatus status)
        {
            Pagination = pagination;
            BrandId = brandId;
            TypeId = typeId;
            Status = status;
        }
        public Pagination Pagination { get; }
        public long BrandId { get; }
        public long TypeId { get; }
        public HealthFacilityStatus Status { get; }
    }
}
