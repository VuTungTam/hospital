using Hospital.Application.Dtos.Symptoms;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Symptoms
{
    public class GetSymptomPagingQuery : BaseAllowAnonymousQuery<PaginationResult<SymptomDto>>
    {
        public GetSymptomPagingQuery(Pagination pagination)
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; set; }
    }
}
