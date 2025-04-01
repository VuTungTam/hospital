using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
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

        public async Task UpdateSymptomsAsync(long bookingId, IEnumerable<long> symptomIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new BookingSymptom().GetTableName()} WHERE {nameof(BookingSymptom.BookingId)} = {bookingId}; ";
            foreach (var symptomId in symptomIds)
            {
                sql += $"INSERT INTO {new BookingSymptom().GetTableName()}(SymptomId, {nameof(BookingSymptom.BookingId)}, CreatedBy, CreatedAt) VALUES ({symptomId}, {bookingId}, {_executionContext.Identity}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }
    }
}
