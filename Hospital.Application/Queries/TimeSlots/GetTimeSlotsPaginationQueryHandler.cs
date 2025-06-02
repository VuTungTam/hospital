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
using Hospital.SharedKernel.Specifications.Interfaces;
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
            ISpecification<TimeSlot> spec = new GetTimeSlotByTimeRuleIdSpecification(request.ServiceId);

            spec = spec.And(new GetTimeSlotIsWalkinSpecification(request.IsWalkin));

            var result = await _timeSlotReadRepository.GetPaginationAsync(request.Pagination, spec: spec, cancellationToken: cancellationToken);

            var slots = _mapper.Map<List<TimeSlotDto>>(result.Data);

            return new PaginationResult<TimeSlotDto>(slots, result.Total);
        }
    }
}
