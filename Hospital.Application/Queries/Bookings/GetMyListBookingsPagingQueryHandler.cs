using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetMyListBookingsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetMyListBookingsPagingQuery, PaginationResult<BookingDto>>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;

        public GetMyListBookingsPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            ITimeSlotReadRepository timeSlotReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository
        ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<PaginationResult<BookingDto>> Handle(GetMyListBookingsPagingQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingReadRepository.GetMyListPagingWithFilterAsync(request.Pagination, request.Status, request.ServiceTypeId, request.Date, cancellationToken);
            var bookingDtos = _mapper.Map<List<BookingDto>>(bookings.Data);
            if (bookings != null && bookings.Data.Any())
            {
                foreach (var bookingDto in bookingDtos)
                {
                    var serviceId = _mapper.Map<long>(bookingDto.ServiceId);
                    var timeSlotId = _mapper.Map<long>(bookingDto.TimeSlotId);
                    var facilityId = _mapper.Map<long>(bookingDto.FacilityId);
                    var profileId = _mapper.Map<long>(bookingDto.HealthProfileId);
                    var service = await _healthServiceReadRepository.GetByIdAsync(serviceId, cancellationToken: cancellationToken);
                    var facility = await _healthFacilityReadRepository.GetByIdAsync(facilityId, cancellationToken: cancellationToken);
                    var timeSlot = await _timeSlotReadRepository.GetByIdAsync(timeSlotId, cancellationToken: cancellationToken);
                    if (service != null)
                    {
                        bookingDto.ServiceNameVn = service.NameVn;
                        bookingDto.ServiceNameEn = service.NameEn;
                        bookingDto.ServiceTypeId = service.TypeId.ToString();
                    }
                    if (facility != null)
                    {
                        bookingDto.FacilityNameVn = facility.NameVn;
                        bookingDto.FacilityNameEn = facility.NameEn;
                    }

                    if (timeSlot != null)
                    {
                        bookingDto.TimeRange = timeSlot.Start.ToString("hh\\:mm") + " - " + timeSlot.End.ToString("hh\\:mm");

                    }
                }
            }

            return new PaginationResult<BookingDto>(bookingDtos, bookings.Total);
        }
    }
}
