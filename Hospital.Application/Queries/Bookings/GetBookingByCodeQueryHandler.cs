using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingByCodeQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingByCodeQuery, BookingDto>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;

        public GetBookingByCodeQueryHandler(
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

        public async Task<BookingDto> Handle(GetBookingByCodeQuery request, CancellationToken cancellationToken)
        {

            var booking = await _bookingReadRepository.GetByCodeAsync(request.Code, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);

            if (bookingDto != null)
            {
                var timeSlotId = _mapper.Map<long>(bookingDto.TimeSlotId);
                var timeSlot = await _timeSlotReadRepository.GetByIdAsync(timeSlotId, cancellationToken: cancellationToken);
                if (timeSlot != null)
                {
                    bookingDto.TimeRange = timeSlot.Start.ToString("hh\\:mm") + " - " + timeSlot.End.ToString("hh\\:mm");

                }
            }

            return bookingDto;
        }
    }
}
