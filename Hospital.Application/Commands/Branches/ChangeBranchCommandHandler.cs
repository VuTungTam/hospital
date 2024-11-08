using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Branches
{
    public class ChangeBranchCommandHandler : BaseCommandHandler, IRequestHandler<ChangeBranchCommand, string>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IAuthRepository _authRepository;

        public ChangeBranchCommandHandler(
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

        public async Task<string> Handle(ChangeBranchCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            if (!_executionContext.IsSuperAdmin() && !_executionContext.BranchIds.Exists(id => request.Id == id))
            {
                throw new ForbiddenException(_localizer["branch_not_permission"]);
            }

            var user = await _authRepository.GetUserByIdAsync(_executionContext.UserId, cancellationToken);

            var roles = user.UserRoles.Select(x => x.Role);
            var accessToken = await _authService.GenerateAccessTokenAsync(user, _authService.GetPermission(user), roles, request.Id, cancellationToken: cancellationToken);

            return accessToken;
        }
    }
}
