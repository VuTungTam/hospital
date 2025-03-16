using AutoMapper;
using Hospital.Application.Dtos.Articles;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticlesPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetArticlesPaginationQuery, PaginationResult<ArticleDto>>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticlesPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IArticleReadRepository articleReadRepository
        ) : base(authService, mapper, localizer)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<PaginationResult<ArticleDto>> Handle(GetArticlesPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _articleReadRepository.GetPaginationWithFilterAsync(request.Pagination, request.Status, request.ExcludeId, request.PostDate, request.ClientSort, cancellationToken);
            var articles = _mapper.Map<List<ArticleDto>>(pagination.Data);

            return new PaginationResult<ArticleDto>(articles, pagination.Total);
        }
    }
}
