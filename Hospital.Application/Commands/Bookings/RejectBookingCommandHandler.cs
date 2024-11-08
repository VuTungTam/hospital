using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class RejectBookingCommandHandler : BaseCommandHandler, IRequestHandler<RejectBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        public RejectBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<Unit> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, ignoreOwner: true, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            if (booking.Status != BookingStatus.Waiting)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_waiting"]);
            }

            booking.Status = BookingStatus.Rejected;

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
