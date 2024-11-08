using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class CancelBookingCommandHandler : BaseCommandHandler, IRequestHandler<CancelBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IRedisCache _redisCache;
        public CancelBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IRedisCache redisCache
            ) : base(eventDispatcher, authService, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
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

            if (booking.Status != BookingStatus.Confirmed)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_confirmed"]);
                //throw new BadRequestException(booking.Status.ToString());
            }

            booking.Status = BookingStatus.Cancel;

            booking.Order = -1;

            _bookingWriteRepository.Update(booking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            var nextBookings = await _bookingReadRepository.GetNextBookingAsync
                (booking, cancellationToken: cancellationToken);

            foreach (var nextBooking in nextBookings)
            {
                nextBooking.Order--;
            }

            var key = BaseCacheKeys.GetQueueOrder(booking.ServiceId, booking.Date, booking.ServiceStartTime, booking.ServiceEndTime);

            var oldOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date, booking.ServiceStartTime, booking.ServiceEndTime, cancellationToken);

            await _bookingWriteRepository.UpdateRangeAsync(nextBookings, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(key, oldOrder - 1, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
