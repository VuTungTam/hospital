using AutoMapper;
using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;


namespace Hospital.Application.Queries.Dashboards.Bookings
{
    public class GetBookingDashboardTrendQueryHandler : BaseQueryHandler, IRequestHandler<GetBookingDashboardTrendQuery, BookingTrend>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetBookingDashboardTrendQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<BookingTrend> Handle(GetBookingDashboardTrendQuery request, CancellationToken cancellationToken)
        {
            return _dashboardRepository.GetBookingTrendAsync(request.FacilityId, cancellationToken);
        }
    }
}
