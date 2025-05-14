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
    public class GetYearlyFacilityBookingStatsQueryHandler : BaseQueryHandler, IRequestHandler<GetYearlyFacilityBookingStatsQuery, YearlyFacilityBookingStats>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetYearlyFacilityBookingStatsQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<YearlyFacilityBookingStats> Handle(GetYearlyFacilityBookingStatsQuery request, CancellationToken cancellationToken)
        {
            if (request.Year <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.PayloadIsNotValid"]);
            }

            return _dashboardRepository.GetYearlyFacilityBookingStatsAsync(request.Year, request.IsOnlyCompleted, cancellationToken);
        }
    }
}
