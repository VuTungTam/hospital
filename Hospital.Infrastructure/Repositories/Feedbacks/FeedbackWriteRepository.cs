using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Feedbacks
{
    public class FeedbackWriteRepository : WriteRepository<Feedback>, IFeedbackWriteRepository
    {
        public FeedbackWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
