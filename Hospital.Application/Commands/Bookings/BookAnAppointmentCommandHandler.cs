using AutoMapper;
using Hangfire;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
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
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
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
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthProfileReadRepository healthProfileReadRepository,
            ITimeSlotReadRepository timeSlotReadRepository,
            IExecutionContext executionContext,
            ICustomerReadRepository customerReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
            _zoneReadRepository = zoneReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _healthProfileReadRepository = healthProfileReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _executionContext = executionContext;
            _customerReadRepository = customerReadRepository;
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

            var profile = await _healthProfileReadRepository.GetByIdAsync(booking.HealthProfileId, cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(booking.Email) || string.IsNullOrWhiteSpace(booking.Phone))
            {
                var customer = await _customerReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

                if (string.IsNullOrWhiteSpace(booking.Email))
                {
                    booking.Email = customer.Email;
                }

                if (string.IsNullOrWhiteSpace(booking.Phone))
                {
                    booking.Phone = customer.Phone;
                }
            }

            if (profile == null)
            {
                throw new BadRequestException("Booking.ServiceNotFound");
            }

            var facility = await _healthFacilityReadRepository.GetByIdAsync(service.FacilityId, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException("Booking.ServiceNotFound");
            }

            booking.HealthProfileName = profile.Name;

            booking.FacilityNameVn = facility.NameVn;

            booking.FacilityNameEn = facility.NameEn;

            var timeSlot = await _timeSlotReadRepository.GetByIdAsync(booking.TimeSlotId, cancellationToken: cancellationToken);

            if (timeSlot == null)
            {
                throw new BadRequestException("Booking.timeSlotNotFound");
            }
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

            booking.DoctorId = service.DoctorId;

            var zones = await _zoneReadRepository.GetZonesByFacilityId(service.FacilityId, cancellationToken: cancellationToken);

            if (zones == null)
            {
                booking.ZoneId = 0;
            }
            else
            {
                var zone = zones.Where(x => x.ZoneSpecialties.Any(s => s.SpecialtyId == service.SpecialtyId)).FirstOrDefault();

                booking.ZoneId = zone.Id;
            }

            _bookingWriteRepository.Add(booking);

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Đặt lịch khám <span class='n-bold'>{booking.Code}</span> thành công lúc <span class='n-bold'>{booking.CreatedAt:HH:mm dd/MM/yyyy}</span>. Số thứ tự của bạn là <span class='n-bold'>{booking.Order}</span></p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };
            var callbackWrapper = new CallbackWrapper();

            await _customerWriteRepository.AddNotificationForCustomerAsync(notification, _executionContext.Identity, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.ScheduleNotificationForCustomerAsync(booking.Id, booking.Date, timeSlot.Start, cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _bookingWriteRepository.ClearCacheAsync(booking, cancellationToken);

            return booking.Id.ToString();
        }
    }
}
