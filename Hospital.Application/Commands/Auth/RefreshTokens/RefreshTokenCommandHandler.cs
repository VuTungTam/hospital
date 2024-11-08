using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.RefreshTokens
{
    public class RefreshTokenCommandHandler : BaseCommandHandler, IRequestHandler<RefreshTokenCommand, LoginResult>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ISystemConfigurationReadRepository _systemConfigurationRepository;

        public RefreshTokenCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IAuthRepository authRepository,
            ISystemConfigurationReadRepository systemConfigurationRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _authRepository = authRepository;
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public async Task<LoginResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _authRepository.GetRefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken);
            if (refreshToken == null)
            {
                throw new BadRequestException(_localizer["auth_refresh_token_is_not_valid"]);
            }

            var user = await _authRepository.GetUserByIdAsync(request.UserId, cancellationToken);

            await _authService.ValidateAccessAndThrowAsync(user, cancellationToken);

            var currentAccessToken = await _authService.GenerateAccessTokenAsync(user, _authService.GetPermission(user), user.UserRoles.Select(x => x.Role), cancellationToken: cancellationToken);
            var sc = await _systemConfigurationRepository.GetAsync(cancellationToken);

            refreshToken.CurrentAccessToken = currentAccessToken;
            refreshToken.ExpiryDate = DateTime.Now.AddSeconds(sc.Session ?? AuthConfig.RefreshTokenTime);

            _authRepository.UpdateRefreshToken(refreshToken);
            await _authRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return new LoginResult { AccessToken = currentAccessToken, RefreshToken = request.RefreshToken };
        }
    }
}
