using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Domain.Models.Auths;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.RefreshTokens
{
    public class RefreshTokenCommandHandler : BaseCommandHandler, IRequestHandler<RefreshTokenCommand, LoginResult>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IActionReadRepository _actionReadRepository;
        private readonly ISystemConfigurationReadRepository _systemConfigurationRepository;
        public RefreshTokenCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IEmployeeReadRepository employeeReadRepository,
            ICustomerReadRepository customerReadRepository,
            IAuthRepository authRepository,
            IActionReadRepository actionReadRepository,
            ISystemConfigurationReadRepository systemConfigurationRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _employeeReadRepository = employeeReadRepository;
            _customerReadRepository = customerReadRepository;
            _authRepository = authRepository;
            _actionReadRepository = actionReadRepository;
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public async Task<LoginResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _authRepository.GetRefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken);
            if (refreshToken == null)
            {
                throw new BadRequestException(_localizer["Authentication.RefreshTokenIsNotValid"]);
            }

            BaseUser account = await _employeeReadRepository.GetByIdIncludedRolesAsync(request.UserId, cancellationToken);
            var isEmployee = true;

            if (account == null)
            {
                isEmployee = false;
                account = await _customerReadRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);
            }
            await _authService.ValidateAccessAndThrowAsync(account, cancellationToken);

            var roles = isEmployee ? (account as Employee).EmployeeRoles.Select(x => x.Role) : new List<Role>();
            var actions = isEmployee ? await _actionReadRepository.GetActionsByEmployeeIdAsync(account.Id, cancellationToken) : new List<ActionWithExcludeValue>();
            var permission = isEmployee ? _authService.GetPermission(account as Employee, actions) : "15";
            var payload = new GenTokenPayload { User = account, Permission = permission, Roles = roles };
            var accessToken = await _authService.GenerateAccessTokenAsync(payload, cancellationToken);
            var sc = await _systemConfigurationRepository.GetAsync(cancellationToken);

            refreshToken.CurrentAccessToken = accessToken;
            refreshToken.ExpiryDate = DateTime.Now.AddSeconds(sc.Session ?? AuthConfig.RefreshTokenTime);

            _authRepository.UpdateRefreshToken(refreshToken);
            await _authRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return new LoginResult { AccessToken = accessToken, RefreshToken = request.RefreshToken };
        }
    }
}
