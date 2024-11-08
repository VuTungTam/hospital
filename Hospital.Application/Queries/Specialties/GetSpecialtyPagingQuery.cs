using Hospital.Application.Dtos.Specialties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Validators;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyPagingQuery : BaseAllowAnonymousQuery<PagingResult<SpecialtyDto>>
    {
        public GetSpecialtyPagingQuery(Pagination pagination)
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; set; }
    }
}
