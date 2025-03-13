using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Newes
{
    public class DeleteNewsCommandHandler : BaseCommandHandler, IRequestHandler<DeleteNewsCommand>
    {
        private readonly INewsReadRepository _newsReadRepository;
        private readonly INewsWriteRepository _newsWriteRepository;

        public DeleteNewsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            INewsReadRepository newsReadRepository,
            INewsWriteRepository newsWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _newsReadRepository = newsReadRepository;
            _newsWriteRepository = newsWriteRepository;
        }

        public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var news = await _newsReadRepository.GetByIdsAsync(request.Ids, _newsReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (news.Any())
            {
                await _newsWriteRepository.DeleteAsync(news, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
