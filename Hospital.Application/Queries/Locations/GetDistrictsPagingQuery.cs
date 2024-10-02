using Hospital.Application.Dtos.Locations;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Locations
{
    public class GetDistrictsPagingQuery : BaseQuery<PagingResult<DistrictDto>>
    {
        public GetDistrictsPagingQuery(Pagination pagination, int provinceId)
        {
            Pagination = pagination;
            ProvinceId = provinceId;
        }

        public Pagination Pagination { get; }
        public int ProvinceId { get; }
    }
}
