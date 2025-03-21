using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Interfaces;
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
    public class ForgotPasswordStep1CommandHandler : BaseCommandHandler, IRequestHandler<ForgotPasswordStep1Command>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IAccountService _accountService;
        private readonly IRedisCache _redisCache;

        public ForgotPasswordStep1CommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IEmployeeReadRepository employeeReadRepository,
            ICustomerReadRepository customerReadRepository,
            IAccountService accountService,
            IRedisCache redisCache
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _employeeReadRepository = employeeReadRepository;
            _customerReadRepository = customerReadRepository;
            _accountService = accountService;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(ForgotPasswordStep1Command request, CancellationToken cancellationToken)
        {
            if (!EmailUtility.IsEmail(request.Email))
            {
                throw new BadRequestException(_localizer["CommonMessage.EmailIsNotValid"]);
            }

            BaseUser user = await _employeeReadRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                user = await _customerReadRepository.GetByEmailAsync(request.Email, cancellationToken);
            }

            _authService.ValidateStateAndThrow(user);

            var cacheEntry = CacheManager.GetBlockSendForgotPwdCacheEntry(user.Email);

            var lockDate = await _redisCache.GetAsync<string>(cacheEntry.Key);

            if (!string.IsNullOrEmpty(lockDate))
            {
                throw new BadRequestException(string.Format(_localizer["Account.PleaseWaitUntil"], lockDate));
            }

            var code = Utility.RandomNumber(6);
            var cacheEntry2 = CacheManager.GetForgotPwdCacheEntry(user.Email);
            var setTasks = new List<Task>
            {
                _redisCache.SetAsync(cacheEntry.Key, $"{DateTime.Now.AddSeconds(cacheEntry.ExpiriesInSeconds):HH:mm:ss}", TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken),
                _redisCache.SetAsync(cacheEntry2.Key, code, TimeSpan.FromSeconds(cacheEntry2.ExpiriesInSeconds), cancellationToken: cancellationToken)
            };

            await Task.WhenAll(setTasks);

            _ = _accountService.SendForgotPwdAsync(user, code, cancellationToken);

            return Unit.Value;
        }
    }
}
