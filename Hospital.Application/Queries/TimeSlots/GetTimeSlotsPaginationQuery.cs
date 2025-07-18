﻿using System.Runtime.CompilerServices;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetTimeSlotsPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<TimeSlotDto>>
    {
        public GetTimeSlotsPaginationQuery(Pagination pagination, long serviceId, bool isWalkin)
        {
            Pagination = pagination;
            ServiceId = serviceId;
            IsWalkin = isWalkin;
        }

        public Pagination Pagination { get; }

        public long ServiceId { get; }

        public bool IsWalkin { get; }

    }
}
