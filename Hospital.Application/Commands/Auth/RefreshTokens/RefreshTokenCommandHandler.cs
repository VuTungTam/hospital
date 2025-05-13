using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Doctors;
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
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.RefreshTokens
{
    public class RefreshTokenCommandHandler : BaseCommandHandler, IRequestHandler<RefreshTokenCommand, LoginResult>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;
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
            IDoctorReadRepository doctorReadRepository,
            IAuthRepository authRepository,
            IActionReadRepository actionReadRepository,
            ISystemConfigurationReadRepository systemConfigurationRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _employeeReadRepository = employeeReadRepository;
            _customerReadRepository = customerReadRepository;
            _doctorReadRepository = doctorReadRepository;
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
            var isDoctor = true;

            if (account == null)
            {
                isEmployee = false;
                var option = new QueryOption
                {
                    IgnoreFacility = true
                };
                account = await _doctorReadRepository.GetByIdAsync(request.UserId, option, cancellationToken);

                if (account == null)
                {
                    isDoctor = false;
                    account = await _customerReadRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);
                }
            }

            await _authService.ValidateAccessAndThrowAsync(account, cancellationToken);

            List<Role> roles;
            List<ActionWithExcludeValue> actions;
            string permission;

            if (isEmployee)
            {
                roles = (account as Employee).EmployeeRoles.Select(x => x.Role).ToList();
                actions = await _actionReadRepository.GetActionsByEmployeeIdAsync(account.Id, cancellationToken);
                permission = _authService.GetPermission(actions);
            }
            else if (isDoctor)
            {
                roles = new List<Role>();
                actions = new List<ActionWithExcludeValue>();
                permission = await _authService.GetDoctorPermission(cancellationToken);
            }
            else
            {
                roles = new List<Role>();
                actions = new List<ActionWithExcludeValue>();
                permission = await _authService.GetCustomerPermission(cancellationToken);
            }

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
