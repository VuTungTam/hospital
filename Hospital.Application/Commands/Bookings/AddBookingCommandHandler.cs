using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class AddBookingCommandHandler : BaseCommandHandler, IRequestHandler<AddBookingCommand, string>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;

        private readonly ISocketService _socketService;
        public AddBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IZoneReadRepository zoneReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IHealthProfileReadRepository healthProfileReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            ICustomerReadRepository customerReadRepository,
            IExecutionContext executionContext,
            ISocketService socketService

        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingWriteRepository = bookingWriteRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _zoneReadRepository = zoneReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _executionContext = executionContext;
            _customerReadRepository = customerReadRepository;
            _healthProfileReadRepository = healthProfileReadRepository;
            _socketService = socketService;
        }

        public async Task<string> Handle(AddBookingCommand request, CancellationToken cancellationToken)
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

            if (!long.TryParse(request.Booking.HealthProfileId, out var healthProfileId) || serviceId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var profile = await _healthProfileReadRepository.GetByIdAsync(healthProfileId, cancellationToken: cancellationToken);

            if (profile == null)
            {
                throw new BadRequestException("Booking.ServiceNotFound");
            }

            var booking = _mapper.Map<Booking>(request.Booking);

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

            booking.Date = booking.Date.AddDays(1);

            booking.Status = BookingStatus.Waiting;

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

            _bookingWriteRepository.Add(booking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);
            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Yêu cầu xét duyệt! Khách hàng <span class='n-bold'></span> vừa đặt lịch khám lúc <span class='n-bold'>{booking.CreatedAt:HH:mm dd/MM/yyyy}</span>. </span></p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.Booking
            };
            var callbackWrapper = new CallbackWrapper();

            await _employeeWriteRepository.AddNotificationForEmployeeAsync(notification, booking.ZoneId, booking.FacilityId, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _socketService.SendNewBooking(booking, cancellationToken);

            await _bookingWriteRepository.ActionAfterAddAsync(cancellationToken);

            await callbackWrapper.Callback();

            return booking.Id.ToString();
        }
    }
}
