using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class AddCustomerCommandHandler : BaseCommandHandler, IRequestHandler<AddCustomerCommand, string>
    {
        private readonly ILocationReadRepository _locationReadRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IRedisCache _redisCache;
        public AddCustomerCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ILocationReadRepository locationReadRepository,
            IUserRepository userRepository,
            ICustomerWriteRepository customerWriteRepository,
            IRedisCache redisCache
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _locationReadRepository = locationReadRepository;
            _userRepository = userRepository;
            _customerWriteRepository = customerWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<string> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(request.Customer, cancellationToken);

            var customer = _mapper.Map<Customer>(request.Customer);

            customer.Wname = await _locationReadRepository.GetNameByIdAsync(customer.Wid, "ward", cancellationToken);
            customer.Dname = await _locationReadRepository.GetNameByIdAsync(customer.Did, "district", cancellationToken);
            customer.Pname = await _locationReadRepository.GetNameByIdAsync(customer.Pid, "province", cancellationToken);

            await _customerWriteRepository.AddCustomerAsync(customer, externalFlow: false, cancellationToken);

            await _customerWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            if (!customer.EmailVerified)
            {
                var verificationCode = Utility.RandomString(16);
                var cacheEntry = CacheManager.GetVerifyAccountCacheEntry(customer.Email, verificationCode);

                await _redisCache.SetAsync(cacheEntry.Key, "-", TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
            }

            return customer.Id.ToString();
        }

        public async Task ValidateAndThrowAsync(CustomerDto customer, CancellationToken cancellationToken)
        {
            var codeExist = await _userRepository.CodeExistAsync(customer.Code, cancellationToken: cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, _localizer["Account.CodeAlreadyExisted"]);
            }

            var phoneExist = await _userRepository.PhoneExistAsync(customer.Phone, cancellationToken: cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
            }

            var emailExist = await _userRepository.EmailExistAsync(customer.Email, cancellationToken: cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
            }
        }
    }
}
