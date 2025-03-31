using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    //Le tan booking
    public class BookAnAppointmentCommandHandler : BaseCommandHandler, IRequestHandler<BookAnAppointmentCommand, string>
    {
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IBookingReadRepository _bookingReadRepository;
        public BookAnAppointmentCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingWriteRepository bookingWriteRepository,
            IBookingReadRepository bookingReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<string> Handle(BookAnAppointmentCommand request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<Booking>(request.Booking);

            booking.Status = BookingStatus.Completed;

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking,cancellationToken);

            return booking.Id.ToString();
        }
    }
}
