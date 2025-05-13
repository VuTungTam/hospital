using AutoMapper;
using Hospital.Application.Models.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetCurrentOrderQueryHandler : BaseQueryHandler, IRequestHandler<GetCurrentOrderQuery, CurrentBookingModel>
    {
        public readonly IBookingReadRepository _bookingReadRepository;
        public GetCurrentOrderQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository
            ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
        }

        public async Task<CurrentBookingModel> Handle(GetCurrentOrderQuery request, CancellationToken cancellationToken)
        {
            return await _bookingReadRepository.GetCurrentAsync(request.ServiceId, request.TimeSlotId, cancellationToken);
        }
    }
}
