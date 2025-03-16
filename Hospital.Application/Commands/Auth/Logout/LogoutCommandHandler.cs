using AutoMapper;
using Hospital.Application.EventBus;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.Logout
{
    public class LogoutCommandHandler : BaseCommandHandler, IRequestHandler<LogoutCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IAuthRepository _authRepository;

        public LogoutCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authService.RevokeAccessTokenAsync(_executionContext.Identity, _executionContext.AccessToken, cancellationToken);

            await _authRepository.RemoveRefreshTokenAsync(_executionContext.AccessToken, cancellationToken);

            await _authRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _eventDispatcher.FireEventAsync(new LogoutDomainEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}
