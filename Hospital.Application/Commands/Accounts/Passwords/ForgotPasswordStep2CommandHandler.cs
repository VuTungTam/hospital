using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts.Passwords
{
    public class ForgotPasswordStep2CommandHandler : BaseCommandHandler, IRequestHandler<ForgotPasswordStep2Command, string>
    {
        private readonly IRedisCache _redisCache;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;

        public ForgotPasswordStep2CommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            IEmployeeReadRepository employeeReadRepository,
            ICustomerReadRepository customerReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _redisCache = redisCache;
            _employeeReadRepository = employeeReadRepository;
            _customerReadRepository = customerReadRepository;
        }

        public async Task<string> Handle(ForgotPasswordStep2Command request, CancellationToken cancellationToken)
        {
            if (!EmailUtility.IsEmail(request.Model.Email))
            {
                throw new BadRequestException(_localizer["CommonMessage.EmailIsNotValid"]);
            }

            if (string.IsNullOrEmpty(request.Model.Code))
            {
                throw new BadRequestException(_localizer["Account.VerificationCodeIsNotValid"]);
            }

            BaseUser user = await _employeeReadRepository.GetByEmailAsync(request.Model.Email, cancellationToken: cancellationToken);
            if (user == null)
            {
                await _customerReadRepository.GetByEmailAsync(request.Model.Email, cancellationToken: cancellationToken);
            }

            _authService.ValidateStateAndThrow(user);

            var cacheEntry = CacheManager.GetForgotPwdCacheEntry(user.Email);
            var valid = await _redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken) == request.Model.Code;
            if (!valid)
            {
                throw new BadRequestException(_localizer["Account.VerificationCodeIsNotValidOrExpired"]);
            }

            var session = Utility.RandomString(16, false);
            var cacheEntry2 = CacheManager.GetForgotPwdSessionCacheEntry(user.Email);

            await _redisCache.SetAsync(cacheEntry2.Key, session, TimeSpan.FromSeconds(cacheEntry2.ExpiriesInSeconds), cancellationToken: cancellationToken);

            return session;
        }
    }
}
