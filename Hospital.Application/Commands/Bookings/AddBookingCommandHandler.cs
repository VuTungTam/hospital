using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class AddBookingCommandHandler : BaseCommandHandler, IRequestHandler<AddBookingCommand, string>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        public AddBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IZoneReadRepository zoneReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            ISymptomReadRepository symptomReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingWriteRepository = bookingWriteRepository;
            _symptomReadRepository = symptomReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _zoneReadRepository = zoneReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
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

            var symptoms = await _symptomReadRepository.GetAsync(cancellationToken: cancellationToken);

            var booking = _mapper.Map<Booking>(request.Booking);

            booking.Status = BookingStatus.Waiting;

            booking.BookingSymptoms = new();

            foreach (var symptom in request.Booking.Symptoms)
            {
                var symptomDb = symptoms.First(x => x.Id + "" == symptom.Id);

                booking.BookingSymptoms.Add(new BookingSymptom
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    SymptomId = symptomDb.Id,
                });
            }

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

            _bookingWriteRepository.Add(booking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Yêu cầu xét duyệt! Khách hàng <span class='n-bold'>{booking.HealthProfile.Name}</span> vừa đặt lịch khám lúc <span class='n-bold'>{booking.Date:HH:mm dd/MM/yyyy}</span>. </span></p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.Booking
            };
            var callbackWrapper = new CallbackWrapper();

            await _employeeWriteRepository.AddBookingNotificationForCoordinatorAsync(notification, booking.ZoneId, booking.FacilityId, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _bookingWriteRepository.ActionAfterAddAsync(cancellationToken);

            await callbackWrapper.Callback();

            return booking.Id.ToString();
        }
    }
}
