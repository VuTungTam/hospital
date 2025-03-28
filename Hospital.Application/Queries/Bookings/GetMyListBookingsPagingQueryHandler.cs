﻿using AutoMapper;
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
    public class GetMyListBookingsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetMyListBookingsPagingQuery, PaginationResult<BookingResponseDto>>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        public GetMyListBookingsPagingQueryHandler(
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

        public async Task<PaginationResult<BookingResponseDto>> Handle(GetMyListBookingsPagingQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingReadRepository.GetMyListPagingWithFilterAsync(request.Pagination, request.Status, request.ServiceId, request.Date, cancellationToken);
            var bookingDtos = _mapper.Map<List<BookingResponseDto>>(bookings.Data);
            if (bookingDtos != null && bookingDtos.Any())
            {
                foreach (var bookingDto in bookingDtos)
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
            }

            return new PaginationResult<BookingResponseDto>(bookingDtos, bookings.Total);
        }
    }
}
