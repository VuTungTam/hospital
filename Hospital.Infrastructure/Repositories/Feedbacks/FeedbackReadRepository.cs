using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Feedbacks
{
    public class FeedbackReadRepository : ReadRepository<Feedback>, IFeedbackReadRepository
    {
        public FeedbackReadRepository(
            IServiceProvider serviceProvider, 
            IStringLocalizer<Resources> localizer, 
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
