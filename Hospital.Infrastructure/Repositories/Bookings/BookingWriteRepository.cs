using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Bookings
{
    public class BookingWriteRepository : WriteRepository<Booking>, IBookingWriteRepository
    {
        public BookingWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddBookingCodeAsync(Booking booking, CancellationToken cancellationToken)
        {
            var table = new Booking().GetTableName();
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            booking.Code = code.ValueString;

            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);
        }

        public async Task ChangeStatusAsync(long bookingId, BookingStatus status, CancellationToken cancellationToken)
        {
            var cacheKey = await SetBlockUpdateCacheAsync(bookingId, cancellationToken);

            var booking = new Booking { Id = bookingId, Status = status };

            _dbContext.Attach(booking);

            _dbContext.Entry(booking).Property(x => x.Status).IsModified = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _redisCache.RemoveAsync(cacheKey.Key, cancellationToken: cancellationToken);

            await RemoveCacheWhenUpdateAsync(bookingId, cancellationToken);
        }

        public Task ActionAfterAddAsync(CancellationToken cancellationToken)
        {
            return base.RemoveCacheWhenAddAsync(cancellationToken);
        }

    }
}
