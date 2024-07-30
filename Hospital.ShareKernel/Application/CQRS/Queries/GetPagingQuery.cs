using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetPagingQuery<T, TResponse> : BaseQuery<PagingResult<TResponse>> where T : BaseEntity
    {
        public GetPagingQuery(Pagination pagination)
        {
            Pagination = pagination;
        }

        public Pagination Pagination { get; }
    }
}
