using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
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
    public class GetMyBookingByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingByIdQuery, BookingResponseDto>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        public GetMyBookingByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            ISymptomReadRepository symptomReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository
        ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _symptomReadRepository = symptomReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<BookingResponseDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(Booking.BookingSymptoms) }
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var bookingDto = _mapper.Map<BookingResponseDto>(booking);

            if (bookingDto != null)
            {
                var serviceId = _mapper.Map<long>(bookingDto.ServiceId);
                var service = await _healthServiceReadRepository.GetByIdAsync(serviceId, _healthServiceReadRepository.DefaultQueryOption, cancellationToken);
                if (service != null)
                {
                    bookingDto.ServiceNameVn = service.NameVn;
                    bookingDto.ServiceNameEn = service.NameEn;
                }

                var symptomIds = _mapper.Map<List<long>>(bookingDto.SymptomIds);
                if (symptomIds?.Any() == true)
                {
                    var symptoms = await _symptomReadRepository.GetByIdsAsync(symptomIds, _symptomReadRepository.DefaultQueryOption, cancellationToken);
                    if (symptoms?.Any() == true)
                    {
                        bookingDto.SymptomNameVns = symptoms.Select(x => x.NameVn).ToList();
                        bookingDto.SymptomNameEns = symptoms.Select(x => x.NameEn).ToList();
                    }
                }

            }

            return bookingDto;
        }
    }
}
