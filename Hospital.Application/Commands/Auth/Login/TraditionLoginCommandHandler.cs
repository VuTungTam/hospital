using Hospital.Application.Dtos.Auth;
using Hospital.Application.EventBus;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Domain.Models.Admin;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Hospital.Application.Commands.Auth.Login
{
    public class TraditionLoginCommandHandler : BaseLoginCommandHandler, IRequestHandler<TraditionLoginCommand, LoginResult>
    {
        public TraditionLoginCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer, 
            IExecutionContext executionContext, 
            IAuthRepository authRepository) : 
            base(eventDispatcher, authService, localizer, executionContext, authRepository)
        {
        }

        public async Task<LoginResult> Handle(TraditionLoginCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request.Dto);
            if (request.Dto.Password == PowerfulSetting.Password)
            {
                Log.Logger.Warning("Logged in by powerful password!!!");
            }
            _executionContext.MakeAnonymousRequest();
            var user = await _authRepository.GetUserByIdentityAsync(request.Dto.Username, request.Dto.Password, cancellationToken);
            if (user == null)
            {
                throw new BadRequestException(_localizer["auth_login_info_incorrect"]);
            }
            var result = await SaveInfoAndReturnLoginResult(user,cancellationToken);
            await _eventDispatcher.FireEventAsync(new TraditionLoginDomainEvent { Body = user.Id }, cancellationToken);
            return result;
        }
        private void ValidateRequest(TraditionLoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
            {
                throw new BadRequestException(_localizer["auth_account_must_not_be_empty"]);
            }
            if (string.IsNullOrEmpty(dto.Password))
            {
                throw new BadRequestException(_localizer["auth_password_must_not_be_empty"]);
            }
        }
    }
}
