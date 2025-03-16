using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Articles
{
    public class PostOrHiddenArticleCommandHandler : BaseCommandHandler, IRequestHandler<PostOrHiddenArticleCommand>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;

        public PostOrHiddenArticleCommandHandler(
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

        public async Task<Unit> Handle(PostOrHiddenArticleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var article = await _articleReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (article == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (request.IsPost)
            {
                if (article.Status == ArticleStatus.Active)
                {
                    throw new BadRequestException(_localizer["Articles.Posted"]);
                }

                if (article.PostDate == null || article.PostDate == default)
                {
                    article.PostDate = DateTime.Now;
                }
                article.Status = ArticleStatus.Active;
            }
            else
            {
                if (article.Status != ArticleStatus.Active)
                {
                    throw new BadRequestException(_localizer["Articles.IsNotActive"]);
                }
                article.Status = ArticleStatus.Hidden;
            }

            //article.AddDomainEvent(new PostOrHiddenArticleDomainEvent(article, request.IsPost));

            await _articleWriteRepository.UpdateAsync(article, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
