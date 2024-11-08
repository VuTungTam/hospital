using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts.Verifications
{
    public class VerifyAccountWithEmailCommandHandler : BaseCommandHandler, IRequestHandler<VerifyAccountWithEmailCommand, LoginResult>
    {
        private readonly IRedisCache _redisCache;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        //private readonly IAuditWriteRepository _auditWriteRepository;
        public VerifyAccountWithEmailCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository
            //IAuditWriteRepository auditWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _redisCache = redisCache;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<LoginResult> Handle(VerifyAccountWithEmailCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            //if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.EmailEncode) || !EmailUtility.IsEmail(model.EmailEncode.ToBase64Decode()))
            //{
            //    throw new BadRequestException(_localizer["account_verification_info_is_not_valid"]);
            //}

            //var email = model.EmailEncode.ToBase64Decode();
            var email = model.EmailEncode;
            var key = BaseCacheKeys.GetVerifyAccountKey(email, model.Code);
            var value = await _redisCache.GetAsync<string>(key, cancellationToken);

            if (string.IsNullOrEmpty(value))
            {
                throw new BadRequestException(_localizer["account_verification_code_has_expired"]);
            }

            var user = await _userReadRepository.GetByEmailAsync(email, cancellationToken);

            _authService.ValidateStateIncludeActiveAndThrow(user);

            user.Status = AccountStatus.Active;
            user.EmailVerified = true;
            await _userWriteRepository.UpdateStatusAsync(user, cancellationToken);
            await _redisCache.RemoveAsync(key, cancellationToken);

            return await _authService.GetLoginResultAsync(user.Id, cancellationToken);
        }
    }
}
