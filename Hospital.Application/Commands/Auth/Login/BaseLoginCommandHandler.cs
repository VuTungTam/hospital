using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.Login
{
    public class BaseLoginCommandHandler : BaseCommandHandler
    {
        protected readonly IExecutionContext _executionContext;
        protected readonly IAuthRepository _authRepository;
        protected readonly IRedisCache _redisCache;
        public BaseLoginCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IAuthRepository authRepository,
            IRedisCache redisCache
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _authRepository = authRepository;
            _redisCache = redisCache;
        }

        protected async Task<LoginResult> SaveInfoAndReturnLoginResult(BaseUser user, CancellationToken cancellationToken)
        {
            try
            {
                await _authService.ValidateAccessAndThrowAsync(user, cancellationToken);
                return await _authService.GetLoginResultAsync(user, cancellationToken);
            }
            finally
            {
                var uid = _executionContext.HttpContext.Request.Headers[HeaderNamesExtension.Uid];
                if (!string.IsNullOrEmpty(uid) && Guid.TryParse(uid, out var _))
                {
                    await _redisCache.RemoveAsync(CacheManager.GetLoginSecure(uid).Key, cancellationToken);
                }
            }
        }

        protected async Task SecureValidateAsync(string username, CancellationToken cancellationToken)
        {
            var uid = _executionContext.HttpContext.Request.Headers[HeaderNamesExtension.Uid];
            if (string.IsNullOrEmpty(uid) || !Guid.TryParse(uid, out var _))
            {
                throw new ForbiddenException("Oh!");
            }

            var cacheEntry = CacheManager.GetLoginSecure(uid);
            var tryLoginCount = await _redisCache.GetAsync<int>(cacheEntry.Key, cancellationToken);

            if (tryLoginCount >= 5)
            {
                throw new ForbiddenException("Too many login attempts. Please try again later.");
            }

            await _redisCache.SetAsync(cacheEntry.Key, tryLoginCount + 1, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken);
        }
    }
}
