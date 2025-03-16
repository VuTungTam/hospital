using AutoMapper;
using Hospital.Application.Dtos.Articles;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleBySlugQueryHandler : BaseQueryHandler, IRequestHandler<GetArticleBySlugQuery, ArticleDto>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticleBySlugQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IArticleReadRepository articleReadRepository
        ) : base(authService, mapper, localizer)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<ArticleDto> Handle(GetArticleBySlugQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Slug))
            {
                return null;
            }

            var article = await _articleReadRepository.GetBySlugAndLangsAsync(request.Slug, request.Langs, cancellationToken: cancellationToken);
            if (article != null)
            {
                article.ViewCount = await _articleReadRepository.GetViewCountAsync(article.Id, cancellationToken);
            }

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
