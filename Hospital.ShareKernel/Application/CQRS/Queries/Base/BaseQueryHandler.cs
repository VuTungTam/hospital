using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Microsoft.Extensions.Localization;
using Polly;
using Polly.Retry;
using Serilog;

namespace Hospital.SharedKernel.Application.CQRS.Queries.Base
{
    public abstract class BaseQueryHandler
    {
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected BaseQueryHandler(IMapper mapper, IStringLocalizer<Resources> localizer)
        {
            _mapper = mapper;
            _localizer = localizer;
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
