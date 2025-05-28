using AutoMapper;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingCountQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingCountQuery, List<TimeSlotBookedDto>>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        public GetBookingCountQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            ITimeSlotReadRepository timeSlotReadRepository
            ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
        }

        public async Task<List<TimeSlotBookedDto>> Handle(GetBookingCountQuery request, CancellationToken cancellationToken)
        {
            var timeSlots = await _timeSlotReadRepository.GetByTimeRuleIdAsync(request.TimeRuleId, cancellationToken);
            var timeSlotBookeds = _mapper.Map<List<TimeSlotBookedDto>>(timeSlots);

            foreach (var ts in timeSlotBookeds)
            {
                ts.Count = await _bookingReadRepository
                .GetBookingCountByTimeSlotId(long.Parse(ts.Id), request.Date, cancellationToken);
                ts.Date = request.Date;
            }

            return timeSlotBookeds;
        }
    }
}
