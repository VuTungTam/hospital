using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class DeleteServiceTimeRuleCommandHandler : BaseCommandHandler, IRequestHandler<DeleteServiceTimeRuleCommand>
    {
        private readonly IServiceTimeRuleReadRepository _erviceTimeRuleReadRepository;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        public DeleteServiceTimeRuleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _erviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
        }

        public async Task<Unit> Handle(DeleteServiceTimeRuleCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var news = await _erviceTimeRuleReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (news.Any())
            {
                await _serviceTimeRuleWriteRepository.DeleteAsync(news, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
