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
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Articles
{
    public class UpdateArticleCommandHandler : BaseCommandHandler, IRequestHandler<UpdateArticleCommand>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;

        public UpdateArticleCommandHandler(
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

        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Article.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var article = await _articleReadRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
            if (article == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var entity = _mapper.Map<Article>(request.Article);
            if (string.IsNullOrEmpty(entity.Slug))
            {
                entity.Slug = Utility.GenerateSlug(entity.Title);
            }

            var isSlugExists = await _articleReadRepository.IsSlugExistsAsync(entity.Id, entity.Slug, cancellationToken);
            if (isSlugExists)
            {
                entity.Slug += "-" + Utility.RandomNumber(6);
            }

            if (entity.Status == ArticleStatus.Active && (entity.PostDate == null || entity.PostDate == default))
            {
                entity.PostDate = DateTime.Now;
            }

            var t = StringHelper.GenerateTOC(entity.Content, entity.Slug);
            var t2 = StringHelper.GenerateTOC(entity.ContentEn, entity.Slug);
            var contentNoHtml = entity.Content.StripHtml(" ").Replace("&#39;", "'").Trim();
            var contentEnNoHtml = entity.ContentEn.StripHtml(" ").Replace("&#39;", "'").Trim();

            entity.Toc = t.Toc;
            entity.Content = t.Html;
            entity.Summary = contentNoHtml[..Math.Min(contentNoHtml.Length, 400)] + "...";

            entity.TocEn = t2.Toc;
            entity.ContentEn = t2.Html;
            entity.SummaryEn= contentEnNoHtml[..Math.Min(contentEnNoHtml.Length, 400)] + "...";

            //entity.AddDomainEvent(new UpdateArticleDomainEvent(entity));

            await _articleWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }


    }
}
