using AutoMapper;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetTimeSlotsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetTimeSlotsPaginationQuery, PaginationResult<TimeSlotDto>>
    {
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        public GetTimeSlotsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            ITimeSlotReadRepository timeSlotReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _timeSlotReadRepository = timeSlotReadRepository;
        }

        public async Task<PaginationResult<TimeSlotDto>> Handle(GetTimeSlotsPaginationQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetTimeSlotByTimeRuleIdSpecification(request.ServiceId);

            if (request.Shift == Shift.Morning)
            {
                spec.And(new GetTimeSlotsAtMorningShiftSpecification());
            }

            if (request.Shift == Shift.Afternoon)
            {
                spec.And(new GetTimeSlotsAtAfternoonShiftSpecification());
            }

            if (request.Shift == Shift.Night)
            {
                spec.And(new GetTimeSlotsAtNightShiftSpecification());
            }

            var result = await _timeSlotReadRepository.GetPaginationAsync(request.Pagination, spec, cancellationToken: cancellationToken);

            var slots = _mapper.Map<List<TimeSlotDto>>(result.Data);

            return new PaginationResult<TimeSlotDto> (slots, result.Total );
        }
    }
}
