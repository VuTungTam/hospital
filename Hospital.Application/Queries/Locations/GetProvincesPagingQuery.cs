using Hospital.Application.Dtos.Locations;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Locations
{
    public class GetProvincesPagingQuery : BaseQuery<PaginationResult<ProvinceDto>>
    {
        public GetProvincesPagingQuery(Pagination pagination) 
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; }
    }
}
