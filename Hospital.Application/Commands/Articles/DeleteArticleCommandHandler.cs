using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Articles
{
    public class DeleteArticleCommandHandler : BaseCommandHandler, IRequestHandler<DeleteArticleCommand>
    {
        private readonly IArticleReadRepository _articleReadRepository;
        private readonly IArticleWriteRepository _articleWriteRepository;

        public DeleteArticleCommandHandler(
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

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var articles = await _articleReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (articles.Any())
            {
                await _articleWriteRepository.DeleteAsync(articles, cancellationToken);

                //await _eventDispatcher.FireEventAsync(new DeleteArticleDomainEvent(articles), cancellationToken);
            }

            return Unit.Value;
        }
    }
}
