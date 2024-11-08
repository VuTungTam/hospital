using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingsPagingQuery, PagingResult<BookingDto>>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        public GetBookingsPagingQueryHandler(
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

        public async Task<PagingResult<BookingDto>> Handle(GetBookingsPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _bookingReadRepository.GetPagingWithFilterAsync(request.Pagination, request.Status, request.ExcludeId, request.Date, request.UserId, true, cancellationToken);
            var bookings = _mapper.Map<List<BookingDto>>(paging.Data);
            if (bookings != null && bookings.Any())
            {
                var symptoms = await _symptomReadRepository.GetAsync(cancellationToken: cancellationToken);
                var services = await _healthServiceReadRepository.GetAsync(cancellationToken: cancellationToken);

                foreach (var booking in bookings)
                {
                    //foreach (var id in booking.SymptomIds)
                    //{
                    //    var symptom = symptoms.FirstOrDefault(x => x.Id == long.Parse(id));
                    //    booking.SymptomNameEns.Add(symptom.NameEn ?? "");
                    //    booking.SymptomNameVns.Add(symptom.NameVn ?? "");
                    //}
                    //booking.ServiceNameVn = services.FirstOrDefault(x => x.Id == long.Parse(booking.ServiceId))?.NameVn ?? "";
                    //booking.ServiceNameEn = services.FirstOrDefault(x => x.Id == long.Parse(booking.ServiceId))?.NameEn ?? "";
                }
            }

            return new PagingResult<BookingDto>(bookings, paging.Total);
        }
    }
}
