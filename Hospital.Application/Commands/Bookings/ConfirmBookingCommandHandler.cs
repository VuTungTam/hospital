using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;
using Ocelot.Values;

namespace Hospital.Application.Commands.Bookings
{
    public class ConfirmBookingCommandHandler : BaseCommandHandler, IRequestHandler<ConfirmBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly IRedisCache _redisCache;
        public ConfirmBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IRedisCache redisCache
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                IgnoreOwner = true,
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);
            
            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            if (booking.Status != BookingStatus.Waiting)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_waiting"]);
            }

            var maxOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date,
                booking.TimeSlotId, cancellationToken);

            var maxSlot = await _serviceTimeRuleReadRepository.GetMaxSlotAsync(booking.ServiceId, booking.Date, cancellationToken);

            if (maxOrder == maxSlot)
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }

            booking.Status = BookingStatus.Confirmed;

            booking.Order = maxOrder + 1;

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);

            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(cacheEntry.Key, booking.Order, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
