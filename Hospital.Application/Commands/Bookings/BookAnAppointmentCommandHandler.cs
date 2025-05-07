using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
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
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IExecutionContext _executionContext;
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
            IHealthServiceReadRepository healthServiceReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IExecutionContext executionContext
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
            _zoneReadRepository = zoneReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _executionContext = executionContext;
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

            var serviceTimeRules = await _serviceTimeRuleReadRepository.GetByServiceIdAsync(booking.ServiceId, cancellationToken: cancellationToken);

            if (serviceTimeRules == null)
            {
                throw new BadRequestException("Chưa có suất khám");
            }
            var timeRule = serviceTimeRules.FirstOrDefault(x => x.DayOfWeek == (int)booking.Date.DayOfWeek);

            if (timeRule == null)
            {
                throw new BadRequestException("Ngày trong tuần không hợp lệ");
            }

            if (maxOrder == timeRule.MaxPatients)
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }

            booking.Order = maxOrder + 1;

            booking.Status = BookingStatus.Confirmed;

            booking.FacilityId = service.FacilityId;

            var option = new QueryOption
            {
                IgnoreFacility = true,
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            //var spec = new GetZoneByFacilityIdSpecification(booking.FacilityId);

            var zones = await _zoneReadRepository.GetAsync(option: option, cancellationToken: cancellationToken);

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

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Yêu cầu xét duyệt! Khách hàng <span class='n-bold'></span> vừa đặt lịch khám lúc <span class='n-bold'>{booking.CreatedAt:HH:mm dd/MM/yyyy}</span>. </span></p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };
            var callbackWrapper = new CallbackWrapper();

            await _customerWriteRepository.AddNotificationForCustomerAsync(notification, _executionContext.Identity, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking, cancellationToken);

            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(cacheEntry.Key, booking.Order, cancellationToken: cancellationToken);

            return booking.Id.ToString();
        }
    }
}
