using Polly.Retry;
using Polly;
using Serilog;
using Hospital.SharedKernel.Runtime.Exceptions;

namespace Hospital.SharedKernel.Application.CQRS.Commands.Base
{
    public abstract class BaseCommandHandler
    {
        public BaseCommandHandler()
        {

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
