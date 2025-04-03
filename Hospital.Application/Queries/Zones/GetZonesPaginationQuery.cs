using Hospital.Application.Dtos.Zones;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Zones
{
    public class GetZonesPaginationQuery : BaseQuery<PaginationResult<ZoneDto>>
    {
        public GetZonesPaginationQuery(Pagination pagination) 
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; }
    }
}
