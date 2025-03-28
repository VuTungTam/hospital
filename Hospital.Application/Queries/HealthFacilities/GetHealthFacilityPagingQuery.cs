﻿using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetHealthFacilityPagingQuery : BaseAllowAnonymousQuery<PaginationResult<HealthFacilityDto>>
    {
        public GetHealthFacilityPagingQuery(Pagination pagination, long typeId, HealthFacilityStatus status)
        {
            Pagination = pagination;
            TypeId = typeId;
            Status = status;
        }
        public Pagination Pagination { get; }
        public long TypeId { get; }
        public HealthFacilityStatus Status { get; }
    }
}
