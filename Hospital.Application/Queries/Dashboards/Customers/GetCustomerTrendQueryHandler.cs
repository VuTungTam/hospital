using AutoMapper;
using Hospital.Application.Models.Dashboards.Customers;
using Hospital.Application.Queries.Dashboards.Customers;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace VetHospital.Application.Queries.Dashboards.Customers
{
    public class GetCustomerTrendQueryHandler : BaseQueryHandler, IRequestHandler<GetCustomerTrendQuery, CustomerTrend>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetCustomerTrendQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<CustomerTrend> Handle(GetCustomerTrendQuery request, CancellationToken cancellationToken)
        {
            return _dashboardRepository.GetCustomerTrendAsync(cancellationToken);
        }
    }
}
