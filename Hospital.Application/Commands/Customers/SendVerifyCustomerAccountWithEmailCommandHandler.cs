using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class SendVerifyCustomerAccountWithEmailCommandHandler : BaseCommandHandler, IRequestHandler<SendVerifyCustomerAccountWithEmailCommand>
    {
        private readonly IAccountService _accountService;
        private readonly IRedisCache _redisCache;
        private readonly ICustomerReadRepository _customerReadRepository;
        public SendVerifyCustomerAccountWithEmailCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IAccountService accountService,
            IRedisCache redisCache,
            ICustomerReadRepository customerReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _accountService = accountService;
            _redisCache = redisCache;
            _customerReadRepository = customerReadRepository;
        }

        public async Task<Unit> Handle(SendVerifyCustomerAccountWithEmailCommand request, CancellationToken cancellationToken)
        {
            if (!EmailUtility.IsEmail(request.Email))
            {
                throw new BadRequestException(_localizer["CommonMessage.EmailIsNotValid"]);
            }

            var cacheEntry = CacheManager.GetBlockSendVerifyAccountCacheEntry(request.Email);
            if (await _redisCache.GetAsync<object>(cacheEntry.Key, cancellationToken) != null)
            {
                throw new BadRequestException(_localizer["Account.EmailHasBeenSentPreviously"]);
            }

            var customer = await _customerReadRepository.GetByEmailAsync(request.Email, cancellationToken);

            _authService.ValidateStateAndThrow(customer);

            if (customer.EmailVerified)
            {
                throw new BadRequestException(_localizer["Account.EmailAlreadyVerified"]);
            }

            var verificationCode = Utility.RandomString(16);
            var cacheEntry2 = CacheManager.GetVerifyAccountCacheEntry(customer.Email, verificationCode);
            var setTasks = new List<Task>
            {
                _redisCache.SetAsync(cacheEntry.Key, 1, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken),
                _redisCache.SetAsync(cacheEntry2.Key, "-", TimeSpan.FromSeconds(cacheEntry2.ExpiriesInSeconds), cancellationToken: cancellationToken),
            };

            await Task.WhenAll(setTasks);

            _ = _accountService.SendVerifyEmailAsync(customer, verificationCode, cancellationToken);

            return Unit.Value;
        }
    }
}

