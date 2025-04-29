using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
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
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly IRedisCache _redisCache;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IZoneReadRepository _zoneReadRepository;
        public BookAnAppointmentCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingWriteRepository bookingWriteRepository,
            IBookingReadRepository bookingReadRepository,
            IRedisCache redisCache,
            IZoneReadRepository zoneReadRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
            _zoneReadRepository = zoneReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<string> Handle(BookAnAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Booking.ServiceId, out var serviceId) || serviceId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var service = await _healthServiceReadRepository.GetByIdAsync(serviceId, cancellationToken: cancellationToken);
            if (service == null)
            {
                throw new BadRequestException("Booking.ServiceNotFound");
            }

            var booking = _mapper.Map<Booking>(request.Booking);

            var maxOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date, booking.TimeSlotId, cancellationToken);

            var maxSlot = await _serviceTimeRuleReadRepository.GetMaxSlotAsync(booking.ServiceId, booking.Date, cancellationToken);

            if (maxOrder == maxSlot)
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }

            booking.Order = maxOrder + 1;

            booking.Status = BookingStatus.Completed;

            booking.FacilityId = service.FacilityId;

            var zones = await _zoneReadRepository.GetAsync(cancellationToken: cancellationToken);

            if (zones == null)
            {
                booking.ZoneId = 0;
            }
            else
            {
                var zone = zones.Where(x => x.ZoneSpecialties.Any(s => s.SpecialtyId == service.SpecialtyId)).FirstOrDefault();

                booking.ZoneId = zone.Id;
            }

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking, cancellationToken);

            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(cacheEntry.Key, booking.Order, cancellationToken: cancellationToken);

            return booking.Id.ToString();
        }
    }
}
