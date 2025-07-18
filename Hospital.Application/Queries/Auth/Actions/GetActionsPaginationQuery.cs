﻿using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Auth.Actions
{
    [RequiredPermission(ActionExponent.Master)]
    public class GetActionsPaginationQuery : BaseQuery<PaginationResult<ActionDto>>
    {
        public GetActionsPaginationQuery(Pagination pagination)
        {
            Pagination = pagination;
        }

        public Pagination Pagination { get; }
    }
}
