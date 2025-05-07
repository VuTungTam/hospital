using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class StartBookingCommandHandler : BaseCommandHandler, IRequestHandler<StartBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IRedisCache _redisCache;
        private readonly IDateService _dateService;
        public StartBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IMapper mapper,
            IDateService dateService
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
            _dateService = dateService;
        }

        public async Task<Unit> Handle(StartBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_confirmed"]);
            }

            booking.StartBooking = _dateService.GetClientTime().TimeOfDay;

            await _bookingWriteRepository.ChangeStatusAsync(request.Id, BookingStatus.Doing, cancellationToken);

            var cacheEntry = CacheManager.GetCurrentOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(cacheEntry.Key, booking.Order, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
