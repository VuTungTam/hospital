using AutoMapper;
using Hospital.Application.EventBus;
using Hospital.Application.Helpers;
using Hospital.Application.Models.Auth;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class RegCustomerAccountCommandHandler : BaseCommandHandler, IRequestHandler<RegCustomerAccountCommand, string>
    {
        private readonly IRedisCache _redisCache;
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public RegCustomerAccountCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            IExecutionContext executionContext,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _redisCache = redisCache;
            _executionContext = executionContext;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<string> Handle(RegCustomerAccountCommand request, CancellationToken cancellationToken)
        {
            await _authService.CheckPasswordLevelAndThrowAsync(request.Account.Password, cancellationToken);

            _executionContext.MakeAnonymousRequest();
            await ValidateAndThrowAsync(request.Account, cancellationToken);

            var customer = _mapper.Map<Customer>(request.Account);

            await _customerWriteRepository.AddCustomerAsync(customer, cancellationToken: cancellationToken);

            await _customerWriteRepository.SaveChangesAsync(cancellationToken);

            await _customerWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            var verificationCode = Utility.RandomString(16);
            var cacheEntry = CacheManager.GetVerifyAccountCacheEntry(customer.Email, verificationCode);

            await _redisCache.SetAsync(cacheEntry.Key, "-", TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);

            _ = _eventDispatcher.FireEventAsync(new RegAccountDomainEvent(customer, verificationCode), cancellationToken);

            return customer.Id.ToString();
        }

        private async Task ValidateAndThrowAsync(RegAccountRequest account, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(account.Phone))
            {
                account.Phone = PhoneHelper.TransferToDomainPhone(account.Phone);
                await InternalValidateAsync(new ExpressionSpecification<Customer>(x => x.Phone == account.Phone), "Account.PhoneAlreadyExists");
            }

            if (!string.IsNullOrEmpty(account.Email))
            {
                await InternalValidateAsync(new ExpressionSpecification<Customer>(x => x.Email == account.Email), "Account.EmailAlreadyExists");
            }

            async Task InternalValidateAsync(ExpressionSpecification<Customer> spec, string localizeKey)
            {
                var entity = await _customerReadRepository.FindBySpecificationAsync(spec, cancellationToken: cancellationToken);
                if (entity != null)
                {
                    throw new BadRequestException(_localizer[localizeKey]);
                }
            }
        }
    }
}
