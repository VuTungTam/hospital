using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        public GetBookingByIdQueryHandler(
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

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var entity = await _bookingReadRepository.GetByIdAsync(request.Id, ignoreOwner: true, cancellationToken: cancellationToken);

            var booking = _mapper.Map<BookingDto>(entity);

            if (booking != null)
            {
                var symptoms = await _symptomReadRepository.GetAsync(cancellationToken: cancellationToken);
                var services = await _healthServiceReadRepository.GetAsync(cancellationToken: cancellationToken);
                
                //foreach (var id in booking.SymptomIds)
                //{
                //    var symptom = symptoms.FirstOrDefault(x => x.Id == long.Parse(id));
                //    booking.SymptomNameEns.Add(symptom.NameEn ?? "");
                //    booking.SymptomNameVns.Add(symptom.NameVn ?? "");
                //}
                //booking.ServiceNameVn = services.FirstOrDefault(x => x.Id == long.Parse(booking.ServiceId))?.NameVn ?? "";
                //booking.ServiceNameEn = services.FirstOrDefault(x => x.Id == long.Parse(booking.ServiceId))?.NameEn ?? "";

            }
            return booking;
        }
    }
}
