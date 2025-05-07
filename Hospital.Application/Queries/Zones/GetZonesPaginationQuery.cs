using Hospital.Application.Dtos.Zones;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Zones
{
    public class GetZonesPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<ZoneDto>>
    {
        public GetZonesPaginationQuery(Pagination pagination, long facilityId)
        {
            Pagination = pagination;
            FacilityId = facilityId;
        }
        public Pagination Pagination { get; }
        public long FacilityId { get; }
    }
}
