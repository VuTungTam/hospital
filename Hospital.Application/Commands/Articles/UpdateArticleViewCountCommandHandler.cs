using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Articles
{
    public class UpdateArticleViewCountCommandHandler : BaseCommandHandler, IRequestHandler<UpdateArticleViewCountCommand>
    {
        private readonly IArticleWriteRepository _articleWriteRepository;

        public UpdateArticleViewCountCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IArticleWriteRepository articleWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _articleWriteRepository = articleWriteRepository;
        }

        public async Task<Unit> Handle(UpdateArticleViewCountCommand request, CancellationToken cancellationToken)
        {
            await _articleWriteRepository.IncreaseViewCountAsync(request.Id, cancellationToken);

            return Unit.Value;
        }
    }
}
