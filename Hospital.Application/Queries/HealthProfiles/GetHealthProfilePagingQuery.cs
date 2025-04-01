using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.HealthProfiles
{
    [RequiredPermission(ActionExponent.ViewProfile)]
    public class GetHealthProfilePagingQuery : BaseQuery<PaginationResult<HealthProfileDto>>
    {
        public GetHealthProfilePagingQuery(Pagination pagination,long userId)
        {
            Pagination = pagination;
            UserId = userId;
        }
        public Pagination Pagination { get; }
        public long UserId { get; }
    }
}
