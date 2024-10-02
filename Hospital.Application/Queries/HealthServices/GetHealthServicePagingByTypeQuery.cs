using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServicePagingByTypeQuery: BaseQuery<PagingResult<HealthServiceDto>>
    {
        public GetHealthServicePagingByTypeQuery(Pagination pagination,long typeId)
        {
            Pagination = pagination;
            TypeId = typeId;
        }
        public Pagination Pagination { get; }
        public long TypeId { get; }
    }
}
