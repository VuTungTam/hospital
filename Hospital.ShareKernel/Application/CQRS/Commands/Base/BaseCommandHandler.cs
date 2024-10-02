using Polly.Retry;
using Polly;
using Serilog;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;

namespace Hospital.SharedKernel.Application.CQRS.Commands.Base
{
    public abstract class BaseCommandHandler
    {
        protected readonly IEventDispatcher _eventDispatcher;
        protected readonly IAuthService _authService;
        protected readonly IStringLocalizer<Resources> _localizer;
        private IStringLocalizer<Resources> localizer;

        public BaseCommandHandler(IEventDispatcher eventDispatcher, IAuthService authService, IStringLocalizer<Resources> localizer)
        {
            _eventDispatcher = eventDispatcher;
            _authService = authService;
            _localizer = localizer;
        }

        protected BaseCommandHandler(IStringLocalizer<Resources> localizer)
        {
            this.localizer = localizer;
        }

        protected virtual AsyncRetryPolicy CreatePolicy(int retryCount = 1)
        {
            return Policy.Handle<Exception>(e =>
            {
                if (e is BadRequestException)
                {
                    return false;
                }
                return true;
            })
            .WaitAndRetryAsync(
                retryCount: retryCount,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timespan, context) =>
                {
                    Log.Logger.Error(exception, "Retry with message: " + exception.Message);
                }
            );
        }
    }
}
