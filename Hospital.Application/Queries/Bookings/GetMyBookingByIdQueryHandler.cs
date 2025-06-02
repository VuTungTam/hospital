using AutoMapper;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetMyBookingByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        public GetMyBookingByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository
        ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, null, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);

            if (bookingDto != null)
            {
                var serviceId = _mapper.Map<long>(bookingDto.ServiceId);
                var service = await _healthServiceReadRepository.GetByIdAsync(serviceId, _healthServiceReadRepository.DefaultQueryOption, cancellationToken);
                if (service != null)
                {
                    bookingDto.ServiceNameVn = service.NameVn;
                    bookingDto.ServiceNameEn = service.NameEn;
                }

            }

            return bookingDto;
        }
    }
}
