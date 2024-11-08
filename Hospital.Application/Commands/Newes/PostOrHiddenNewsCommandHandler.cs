using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Newes
{
    public class PostOrHiddenNewsCommandHandler : BaseCommandHandler, IRequestHandler<PostOrHiddenNewsCommand>
    {
        private readonly INewsReadRepository _newsReadRepository;
        private readonly INewsWriteRepository _newsWriteRepository;

        public PostOrHiddenNewsCommandHandler(
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

        public async Task<Unit> Handle(PostOrHiddenNewsCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var news = await _newsReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken) ?? throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);

            if (request.IsPost)
            {
                if (news.Status == NewsStatus.Active)
                {
                    throw new BadRequestException(_localizer["news_posted"]);
                }

                if (news.PostDate == null || news.PostDate == default)
                {
                    news.PostDate = DateTime.Now;
                }
                news.Status = NewsStatus.Active;
            }
            else
            {
                if (news.Status != NewsStatus.Active)
                {
                    throw new BadRequestException(_localizer["news_is_not_active"]);
                }
                news.Status = NewsStatus.Hidden;
            }

            await _newsWriteRepository.UpdateAsync(news, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
