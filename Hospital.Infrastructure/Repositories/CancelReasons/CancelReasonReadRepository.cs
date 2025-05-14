using Hospital.Application.Queries.CancelReasons;
using Hospital.Application.Repositories.Interfaces.CancelReasons;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Domain.Specifications.CancelReasons;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.CancelReasons
{
    public class CancelReasonReadRepository : ReadRepository<CancelReason>, ICancelReasonReadRepository
    {
        public CancelReasonReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<CancelReason> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.BookingId == bookingId, cancellationToken);
        }
    }
}