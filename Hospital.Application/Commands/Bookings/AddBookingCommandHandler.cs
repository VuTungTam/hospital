using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class AddBookingCommandHandler : BaseCommandHandler, IRequestHandler<AddBookingCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IBookingWriteRepository _bookingWriteRepository;

        public AddBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingWriteRepository bookingWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<string> Handle(AddBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<Booking>(request.Booking);
            if (booking.Status == BookingStatus.None)
            {
                booking.Status = BookingStatus.Waiting;
            }

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking, cancellationToken);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            return booking.Id.ToString();
        }
    }
}
