using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class CheckCurrentBookingQueryHandler : BaseQueryHandler, IRequestHandler<CheckCurrentBookingQuery, bool>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly IDateService _dateService;

        public CheckCurrentBookingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            ITimeSlotReadRepository timeSlotReadRepository,
            IDateService dateService
        ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _dateService = dateService;
        }

        public async Task<bool> Handle(CheckCurrentBookingQuery request, CancellationToken cancellationToken)
        {

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }
            var timeSlot = await _timeSlotReadRepository.GetByIdAsync(booking.TimeSlotId, cancellationToken: cancellationToken);
            if (timeSlot == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                return false;
            }

            var now = _dateService.GetClientTime();
            if (booking.Date != now.Date)
            {
                return false;
            }
            if (now.TimeOfDay < timeSlot.Start || now.TimeOfDay > timeSlot.End)
            {
                return false;
            }

            return true;
        }
    }
}
