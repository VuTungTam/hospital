using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.EventBus;
using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts.Reg
{
    public class RegAccountCommandHandler : BaseCommandHandler, IRequestHandler<RegAccountCommand, string>
    {
        private readonly IRedisCache _redisCache;
        private readonly IExecutionContext _executionContext;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IMapper _mapper;
        public RegAccountCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IExecutionContext executionContext,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            IMapper mapper
        ) : base(eventDispatcher, authService, localizer)
        {
            _redisCache = redisCache;
            _executionContext = executionContext;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegAccountCommand request, CancellationToken cancellationToken)
        {
            await _authService.CheckPasswordLevelAndThrowAsync(request.Account.Password, cancellationToken);

            _executionContext.MakeAnonymousRequest();
            await ValidateAndThrowAsync(request.Account, cancellationToken);

            var user = _mapper.Map<User>(request.Account);

            await _userWriteRepository.AddCustomerAsync(user, cancellationToken: cancellationToken);

            await _userWriteRepository.SaveChangesAsync(cancellationToken);

            await _userWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            var verificationCode = Utility.RandomString(16);
            var key = BaseCacheKeys.GetVerifyAccountKey(user.Email, verificationCode);

            await _redisCache.SetAsync(key, "-", TimeSpan.FromSeconds(AppCacheTime.VerifyAccountWithEmailCode), cancellationToken: cancellationToken);

            _ = _eventDispatcher.FireEventAsync(new RegAccountDomainEvent(user, verificationCode), cancellationToken);

            return user.Id.ToString();
        }
        private async Task ValidateAndThrowAsync(RegAccountDto account, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(account.Phone))
            {
                account.Phone = PhoneHelper.TransferToDomainPhone(account.Phone);
                await InternalValidateAsync(new ExpressionSpecification<User>(x => x.Phone == account.Phone), "account_phone_already_exist");
            }

            if (!string.IsNullOrEmpty(account.Email))
            {
                await InternalValidateAsync(new ExpressionSpecification<User>(x => x.Email == account.Email), "account_email_already_exist");
            }

            async Task InternalValidateAsync(ExpressionSpecification<User> spec, string localizeKey)
            {
                var entity = await _userReadRepository.FindBySpecificationAsync(spec, _userReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
                if (entity != null)
                {
                    throw new BadRequestException(_localizer[localizeKey]);
                }
            }
        }
    }
}
