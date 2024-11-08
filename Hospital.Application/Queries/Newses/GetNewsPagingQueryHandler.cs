using AutoMapper;
using Hospital.Application.Dtos.Newes;
using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetNewsPagingQuery, PagingResult<NewsDto>>
    {
        private readonly INewsReadRepository _newsReadRepository;

        public GetNewsPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            INewsReadRepository newsReadRepository
        ) : base(authService, mapper, localizer)
        {
            _newsReadRepository = newsReadRepository;
        }

        public async Task<PagingResult<NewsDto>> Handle(GetNewsPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _newsReadRepository.GetPagingWithFilterAsync(request.Pagination, request.Status, request.ExcludeId, request.PostDate, request.ClientSort, cancellationToken);
            var newses = _mapper.Map<List<NewsDto>>(paging.Data);
            foreach (var news in newses)
            {
                news.Summary = news.Content?.StripHtml()[..Math.Min(news.Content.StripHtml().Length, 400)] ?? "";
                news.SummaryEn = news.ContentEn?.StripHtml()[..Math.Min(news.ContentEn.StripHtml().Length, 400)] ?? "";

                news.Content = "";
                news.ContentEn = "";
            }

            return new PagingResult<NewsDto>(newses, paging.Total);
        }
    }
}
