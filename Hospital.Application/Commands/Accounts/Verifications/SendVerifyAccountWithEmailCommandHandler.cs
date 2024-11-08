using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Accounts.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts.Verifications
{
    public class SendVerifyAccountWithEmailCommandHandler : BaseCommandHandler, IRequestHandler<SendVerifyAccountWithEmailCommand>
    {
        private readonly IAccountService _accountService;
        private readonly IRedisCache _redisCache;
        private readonly IUserReadRepository _userReadRepository;

        public SendVerifyAccountWithEmailCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IAccountService accountService,
            IRedisCache redisCache,
            IUserReadRepository userReadRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _accountService = accountService;
            _redisCache = redisCache;
            _userReadRepository = userReadRepository;
        }

        public async Task<Unit> Handle(SendVerifyAccountWithEmailCommand request, CancellationToken cancellationToken)
        {
            if (!EmailUtility.IsEmail(request.Email))
            {
                throw new BadRequestException(_localizer["common_email_is_not_valid"]);
            }

            var blockSendKey = BaseCacheKeys.GetBlockSendVerifyAccountKey(request.Email);
            if ((await _redisCache.GetAsync<object>(blockSendKey, cancellationToken)) != null)
            {
                throw new BadRequestException(_localizer["account_email_has_been_sent_previously"]);
            }

            var user = await _userReadRepository.GetByEmailAsync(request.Email, cancellationToken);

            _authService.ValidateStateAndThrow(user);

            if (user.EmailVerified)
            {
                throw new BadRequestException(_localizer["account_email_already_verified"]);
            }

            var verificationCode = Utility.RandomString(16);
            var key = BaseCacheKeys.GetVerifyAccountKey(user.Email, verificationCode);
            var setTasks = new List<Task>
            {
                _redisCache.SetAsync(key, "-", TimeSpan.FromSeconds(AppCacheTime.VerifyAccountWithEmailCode), cancellationToken: cancellationToken),
                _redisCache.SetAsync(blockSendKey, 1, TimeSpan.FromSeconds(AppCacheTime.LockSendingVerifyAccountWithEmail), cancellationToken: cancellationToken)
            };

            await Task.WhenAll(setTasks);

            //_ = _accountService.SendVerifyEmailAsync(user, verificationCode, cancellationToken);

            return Unit.Value;
        }
    }
}
