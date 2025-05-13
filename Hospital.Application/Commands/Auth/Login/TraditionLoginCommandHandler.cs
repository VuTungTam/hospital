using AutoMapper;
using Hospital.Application.EventBus;
using Hospital.Application.Models.Auth;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Domain.Models.Admin;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Hospital.Application.Commands.Auth.Login
{
    public class TraditionLoginCommandHandler : BaseLoginCommandHandler, IRequestHandler<TraditionLoginCommand, LoginResult>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;

        public TraditionLoginCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IAuthRepository authRepository,
            IRedisCache redisCache,
            IEmployeeReadRepository employeeReadRepository,
            ICustomerReadRepository customerReadRepository,
            IDoctorReadRepository doctorReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper, executionContext, authRepository, redisCache)
        {
            _employeeReadRepository = employeeReadRepository;
            _customerReadRepository = customerReadRepository;
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<LoginResult> Handle(TraditionLoginCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request.Dto);

            //await SecureValidateAsync(request.Dto.Username, cancellationToken);

            if (request.Dto.Password == PowerfulSetting.Password)
            {
                Log.Logger.Warning("Logged in by powerful password!!!");
            }

            _executionContext.MakeAnonymousRequest();

            BaseUser account = await _employeeReadRepository.GetLoginByEmailAsync(request.Dto.Username, request.Dto.Password, cancellationToken: cancellationToken);

            if (account == null)
            {
                account = await _customerReadRepository.GetLoginByEmailAsync(request.Dto.Username, request.Dto.Password, cancellationToken: cancellationToken);
            }

            if (account == null)
            {
                account = await _doctorReadRepository.GetLoginByEmailAsync(request.Dto.Username, request.Dto.Password, cancellationToken: cancellationToken);
            }

            if (account == null)
            {
                throw new BadRequestException(_localizer["Authentication.LoginInformationIsIncorrect"]);
            }

            var result = await SaveInfoAndReturnLoginResult(account, cancellationToken);

            await _eventDispatcher.FireEventAsync(new TraditionLoginDomainEvent { Body = account.Id }, cancellationToken);

            return result;
        }

        private void ValidateRequest(TraditionLoginRequest dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
            {
                throw new BadRequestException(_localizer["Authentication.AccountMustNotBeEmpty"]);
            }
            if (string.IsNullOrEmpty(dto.Password))
            {
                throw new BadRequestException(_localizer["Authentication.PasswordMustNotBeEmpty"]);
            }
        }
    }
}
