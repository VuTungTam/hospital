using AutoMapper;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Polly;
using Polly.Retry;
using Serilog;

namespace Hospital.SharedKernel.Application.CQRS.Queries.Base
{
    public abstract class BaseQueryHandler
    {
        protected readonly IMapper _mapper;
        protected BaseQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        protected virtual AsyncRetryPolicy CreatePolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(
                             retryCount: 1,
                             sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                             onRetry: (exception, timespan, context) =>
                             {
                                 Log.Logger.Error(exception, "Retry with message: " + exception.Message);
                             }
                         );
        }
    }
    

}
