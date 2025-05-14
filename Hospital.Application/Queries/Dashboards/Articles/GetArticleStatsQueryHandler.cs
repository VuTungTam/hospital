using AutoMapper;
using Hospital.Application.Models.Dashboards.Articles;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Dashboards.Articles
{
    public class GetArticleStatsQueryHandler : BaseQueryHandler, IRequestHandler<GetArticleStatsQuery, ArticleStats>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetArticleStatsQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDashboardRepository dashboardRepository
        ) : base(authService, mapper, localizer)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<ArticleStats> Handle(GetArticleStatsQuery request, CancellationToken cancellationToken)
        {
            return _dashboardRepository.GetArticleStatsAsync(request.Top, cancellationToken);
        }
    }
}
