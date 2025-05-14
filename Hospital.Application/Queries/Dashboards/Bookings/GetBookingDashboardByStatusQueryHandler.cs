using AutoMapper;
using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Dashboards.Bookings
{
    public class GetBookingDashboardByStatusQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingDashboardByStatusQuery, BookingStatusStats>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetBookingDashboardByStatusQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<BookingStatusStats> Handle(GetBookingDashboardByStatusQuery request, CancellationToken cancellationToken)
        {
            if (request.FacilityId < 0 || request.Year <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.PayloadIsNotValid"]);
            }

            return _dashboardRepository.GetBookingStatusStatsAsync(request.FacilityId, request.Year, cancellationToken);
        }
    }
}
