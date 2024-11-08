using AutoMapper;
using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Users
{
    public class CheckAccessByPasswordQueryHandler : BaseCommandHandler, IRequestHandler<CheckAccessByPasswordQuery, string>
    {
        private readonly IUserReadRepository _userReadRepository;
        //private readonly ISmsService _smsService;
        private readonly IRedisCache _redisCache;
        public CheckAccessByPasswordQueryHandler(
             IEventDispatcher eventDispatcher,
             IAuthService authService,
             IStringLocalizer<Resources> localizer,
             IUserReadRepository userReadRepository,
             //ISmsService smsService,
             IRedisCache redisCache
         ) : base(eventDispatcher, authService, localizer)
        {
        }

        public async Task<string> Handle(CheckAccessByPasswordQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                throw new BadRequestException(_localizer["common_bad_query_param"]);
            }

            if (request.Username.ToLower() == "admin")
            {
                return "y";
            }

            var phone = PhoneHelper.TransferToDomainPhone(request.Username);
            if (!SmsUtility.IsVietnamesePhone(phone))
            {
                throw new BadRequestException(_localizer["auth_phone_is_not_valid"]);
            }

            var user = await _userReadRepository.GetByPhoneAsync(phone, cancellationToken);
            if (user == null)
            {
                return "not_exists";
            }

            await _authService.ValidateAccessAndThrowAsync(user, cancellationToken);

            var result = string.IsNullOrEmpty(user.PasswordHash) ? "n" : "y";
            if (result == "n" && request.AutoSendOtp)
            {
                var otp = Utility.RandomNumber(4);
                var key = AppCacheKeys.GetLoginOtpKey(phone);

                await _redisCache.SetAsync(key, otp, TimeSpan.FromSeconds(AppCacheTime.LoginWithPhoneUsingOtp), cancellationToken: cancellationToken);

                //var smsRequest = new SmsRequest
                //{
                //    Phone = user.Phone,
                //    Message = $"{otp} is your sign-in code"
                //};
               // await _smsService.SendAsync(smsRequest, true, cancellationToken);
            }

            return result;
        }
    }
}
