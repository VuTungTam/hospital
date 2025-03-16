using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class VerifyCustomerAccountWithEmailCommandHandler : BaseCommandHandler, IRequestHandler<VerifyCustomerAccountWithEmailCommand, LoginResult>
    {
        private readonly IRedisCache _redisCache;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public VerifyCustomerAccountWithEmailCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _redisCache = redisCache;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<LoginResult> Handle(VerifyCustomerAccountWithEmailCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.EmailEncode) || !EmailUtility.IsEmail(model.EmailEncode.ToBase64Decode()))
            {
                throw new BadRequestException(_localizer["Account.VerificationInfomationIsNotValid"]);
            }

            var email = model.EmailEncode.ToBase64Decode();
            var cacheEntry = CacheManager.GetVerifyAccountCacheEntry(email, model.Code);
            var value = await _redisCache.GetAsync<string>(cacheEntry.Key, cancellationToken);

            if (string.IsNullOrEmpty(value))
            {
                throw new BadRequestException(_localizer["Account.VerificationCodeHasExpired"]);
            }

            var customer = await _customerReadRepository.GetByEmailAsync(email, cancellationToken);

            if (customer.EmailVerified)
            {
                throw new BadRequestException(_localizer["Account.AlreadyVerified"]);
            }

            customer.EmailVerified = true;

            await _customerWriteRepository.UpdateStatusAsync(customer, cancellationToken);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);

            var cacheEntry2 = CacheManager.DbSystemIdCacheEntry<Customer>(customer.Id);

            await _redisCache.RemoveAsync(cacheEntry2.Key, cancellationToken);

            return await _authService.GetLoginResultAsync(customer.Id, cancellationToken);
        }
    }
}
