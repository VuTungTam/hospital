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
    public class GetTopFacilityPerYearQueryHandler : BaseQueryHandler, IRequestHandler<GetTopFacilityPerYearQuery, TopFacility>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetTopFacilityPerYearQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<TopFacility> Handle(GetTopFacilityPerYearQuery request, CancellationToken cancellationToken)
        {
            if (request.Year <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.PayloadIsNotValid"]);
            }

            return _dashboardRepository.GetTopFacilityPerYear(request.Year, cancellationToken);
        }
    }
}
