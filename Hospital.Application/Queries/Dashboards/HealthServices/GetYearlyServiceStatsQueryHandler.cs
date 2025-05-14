using AutoMapper;
using Hospital.Application.Models.Dashboards.Services;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Dashboards.HealthServices
{
    public class GetYearlyServiceStatsQueryHandler : BaseQueryHandler, IRequestHandler<GetYearlyServiceStatsQuery, YearlyServiceStats>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetYearlyServiceStatsQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<YearlyServiceStats> Handle(GetYearlyServiceStatsQuery request, CancellationToken cancellationToken)
        {
            if (request.FacilityId < 0 || request.Year <= 0 || request.Top <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.PayloadIsNotValid"]);
            }

            return _dashboardRepository.GetYearlyServiceStatsAsync(request.FacilityId, request.Year, request.IsOnlyCompleted, request.Top, cancellationToken);
        }
    }
}
