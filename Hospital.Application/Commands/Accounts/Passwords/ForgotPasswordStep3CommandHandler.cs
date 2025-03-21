using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts.Passwords
{
    public class ForgotPasswordStep3CommandHandler : BaseCommandHandler, IRequestHandler<ForgotPasswordStep3Command>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IRedisCache _redisCache;

        public ForgotPasswordStep3CommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IEmployeeReadRepository employeeReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IRedisCache redisCache
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _employeeReadRepository = employeeReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(ForgotPasswordStep3Command request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Dto.Email))
            {
                throw new BadRequestException(_localizer["CommonMessage.EmailIsNotValid"]);
            }
            if (string.IsNullOrEmpty(request.Dto.NewPassword))
            {
                throw new BadRequestException(_localizer["Account.NewPasswordMustNotBeEmpty"]);
            }
            if (string.IsNullOrEmpty(request.Dto.Session))
            {
                throw new BadRequestException(_localizer["Account.VerificationCodeIsNotValid"]);
            }

            var isEmployee = true;
            BaseUser user = await _employeeReadRepository.GetByEmailAsync(request.Dto.Email, cancellationToken);

            if (user == null)
            {
                isEmployee = false;
                user = await _customerReadRepository.GetByEmailAsync(request.Dto.Email, cancellationToken);
            }

            _authService.ValidateStateAndThrow(user);

            await _authService.CheckPasswordLevelAndThrowAsync(request.Dto.NewPassword, cancellationToken);

            var cacheEntry = CacheManager.GetForgotPwdSessionCacheEntry(user.Email);
            var valid = await _redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken) == request.Dto.Session;
            if (!valid)
            {
                throw new BadRequestException(_localizer["Account.VerificationCodeIsExpired"]);
            }

            user.Password = request.Dto.NewPassword;
            user.HashPassword();

            if (isEmployee)
            {
                await _employeeWriteRepository.UpdateAsync(user as Employee, cancellationToken: cancellationToken);
            }
            else
            {
                await _customerWriteRepository.UpdateAsync(user as Customer, cancellationToken: cancellationToken);
            }

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);

            return Unit.Value;
        }
    }
}
