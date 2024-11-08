using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.Login
{
    public class BaseLoginCommandHandler : BaseCommandHandler
    {
        protected readonly IExecutionContext _executionContext;
        protected readonly IAuthRepository _authRepository;
        public BaseLoginCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IExecutionContext executionContext,
            IAuthRepository authRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _executionContext = executionContext;
            _authRepository = authRepository;
        }
        protected async Task<LoginResult> SaveInfoAndReturnLoginResult(User user, CancellationToken cancellationToken)
        {
            await _authService.ValidateAccessAndThrowAsync(user, cancellationToken);
            return await _authService.GetLoginResultAsync(user, cancellationToken);
        }
    }
}
