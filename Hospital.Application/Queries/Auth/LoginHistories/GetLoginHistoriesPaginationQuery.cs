using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Auth.LoginHistories
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetLoginHistoriesPaginationQuery : BaseQuery<PaginationResult<LoginHistoryDto>>
    {
        public GetLoginHistoriesPaginationQuery(Pagination pagination, long userId)
        {
            Pagination = pagination;
            UserId = userId;
        }

        public Pagination Pagination { get; }
        public long UserId { get; }
    }
}
