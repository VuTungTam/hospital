using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetCurrentOrderQueryHandler : BaseQueryHandler, IRequestHandler<GetCurrentOrderQuery, int>
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

        public async Task<int> Handle(GetCurrentOrderQuery request, CancellationToken cancellationToken)
        {
            return await _bookingReadRepository.GetCurrentAsync(request.ServiceId, cancellationToken);
        }
    }
}
