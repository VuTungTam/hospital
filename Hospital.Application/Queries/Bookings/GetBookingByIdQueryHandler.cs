using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;

        public GetBookingByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthProfileReadRepository healthProfileReadRepository,
            ITimeSlotReadRepository timeSlotReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository
        ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _healthProfileReadRepository = healthProfileReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                IgnoreOwner = true,
                IgnoreDoctor = false,
                IgnoreFacility = false,
                IgnoreZone = false,
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);

            if (bookingDto != null)
            {
                var serviceId = _mapper.Map<long>(bookingDto.ServiceId);
                var timeSlotId = _mapper.Map<long>(bookingDto.TimeSlotId);
                var facilityId = _mapper.Map<long>(bookingDto.FacilityId);
                var profileId = _mapper.Map<long>(bookingDto.HealthProfileId);
                var service = await _healthServiceReadRepository.GetByIdAsync(serviceId, cancellationToken: cancellationToken);
                var facility = await _healthFacilityReadRepository.GetByIdAsync(facilityId, cancellationToken: cancellationToken);
                var profile = await _healthProfileReadRepository.GetByIdAsync(facilityId, cancellationToken: cancellationToken);
                var timeSlot = await _timeSlotReadRepository.GetByIdAsync(timeSlotId, cancellationToken: cancellationToken);
                if (service != null)
                {
                    bookingDto.ServiceNameVn = service.NameVn;
                    bookingDto.ServiceNameEn = service.NameEn;
                    bookingDto.TimeRange = timeSlot.Start.ToString("hh\\:mm") + " - " + timeSlot.End.ToString("hh\\:mm");
                    bookingDto.FacilityNameVn = facility.NameVn;
                    bookingDto.FacilityNameEn = facility.NameEn;
                    bookingDto.HealthProfileName = profile.Name;
                }
            }

            return bookingDto;
        }
    }
}
