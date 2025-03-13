using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingCountQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingCountQuery, int>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        public GetBookingCountQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository
            ) : base(authService, mapper, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
        }

        public async Task<int> Handle(GetBookingCountQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetBookingsByStatusSpecification(BookingStatus.Confirmed);
            
            var quantity = await _bookingReadRepository.GetCountBySpecAsync(spec,_bookingReadRepository.DefaultQueryOption, cancellationToken);
            
            return quantity;
        }
    }
}
