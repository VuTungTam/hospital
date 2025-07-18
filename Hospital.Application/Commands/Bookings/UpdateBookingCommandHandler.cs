﻿using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Entities.Bookings;
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
    public class UpdateBookingCommandHandler : BaseCommandHandler, IRequestHandler<UpdateBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;

        public UpdateBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Booking.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var booking = await _bookingReadRepository.GetByIdAsync(id, _bookingReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            if (booking.Status != BookingStatus.Waiting)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_waiting"]);
            }

            var entity = _mapper.Map<Booking>(request.Booking);

            await _bookingWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
