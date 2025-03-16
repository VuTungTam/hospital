using Hospital.Application.Dtos.Locations;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Locations
{
    public class GetWardsPagingQuery : BaseQuery<PaginationResult<WardDto>>
    {
        public GetWardsPagingQuery(Pagination pagination, int districtId) 
        {
            Pagination = pagination;
            DistrictId = districtId;
        }
        public Pagination Pagination { get; set; }
        public int DistrictId { get; set; }
    }
}
