using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Queries.Auth.LoginHistories
{
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
