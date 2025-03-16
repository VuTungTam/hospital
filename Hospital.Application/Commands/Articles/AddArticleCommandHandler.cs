using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Helpers;
using Hospital.SharedKernel.Libraries.Utils;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Articles
{
    public class AddArticleCommandHandler : BaseCommandHandler, IRequestHandler<AddArticleCommand, string>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;

        public AddArticleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IArticleReadRepository articleReadRepository,
            IArticleWriteRepository articleWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _articleReadRepository = articleReadRepository;
            _articleWriteRepository = articleWriteRepository;
        }

        public async Task<string> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(request.Article);

            if (article.Status == ArticleStatus.Active && (article.PostDate == null || article.PostDate == default))
            {
                article.PostDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(article.Slug))
            {
                article.Slug = Utility.GenerateSlug(article.Title);
            }

            var isSlugExists = await _articleReadRepository.IsSlugExistsAsync(0, article.Slug, cancellationToken);
            if (isSlugExists)
            {
                article.Slug += "-" + Utility.RandomNumber(6);
            }


            var t = StringHelper.GenerateTOC(article.Content, article.Slug);
            var t2 = StringHelper.GenerateTOC(article.ContentEn, article.Slug);
            var contentNoHtml = article.Content.StripHtml(" ").Replace("&#39;", "'").Trim();
            var contentEnNoHtml = article.ContentEn.StripHtml(" ").Replace("&#39;", "'").Trim();

            article.Toc = t.Toc;
            article.Content = t.Html;
            article.Summary = contentNoHtml[..Math.Min(contentNoHtml.Length, 400)] + "...";

            article.TocEn = t2.Toc;
            article.ContentEn = t2.Html;
            article.SummaryEn= contentEnNoHtml[..Math.Min(contentEnNoHtml.Length, 400)] + "...";

            await _articleWriteRepository.AddAsync(article, cancellationToken);

            return article.Id.ToString();
        }
    }
}
