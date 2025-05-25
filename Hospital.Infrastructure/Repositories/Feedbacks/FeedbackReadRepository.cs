using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Feedback> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken)
        {
            var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.BookingId == bookingId, cancellationToken);

            return feedback;
        }
    }
}
