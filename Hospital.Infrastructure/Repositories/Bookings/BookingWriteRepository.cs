using Hangfire;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
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

        public Task ActionAfterAddAsync(Booking booking, CancellationToken cancellationToken)
        {
            return base.RemoveCacheWhenAddAsync(booking, cancellationToken);
        }

        public async Task ClearCacheAsync(Booking booking, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            var cacheEntry2 = CacheManager.GetBookingCountByTimeSlotIdCacheEntry(booking.TimeSlotId, booking.Date);

            await _redisCache.RemoveAsync(cacheEntry2.Key, cancellationToken: cancellationToken);
        }
        public Task ScheduleNotificationForCustomerAsync(long bookingId, string code, DateTime appointmentDate, TimeSpan startTime, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;
            DateTime appointmentDateTime = appointmentDate.Date + startTime;

            TimeSpan timeUntilAppointment = appointmentDateTime - now;
            DateTime notiDateTime1 = appointmentDateTime.AddMinutes(-30);


            DateTime fiveAM = appointmentDate.Date.AddHours(5);
            TimeSpan timeUntilFiveAM = fiveAM - now;

            var notification = new Notification
            {
                Data = bookingId.ToString(),
                IsUnread = true,
                Description = $"<p>Bạn có lịch khám  <span class='n-bold'>{code}</span> lúc <span class='n-bold'>{startTime:hh\\:mm}</span> hôm nay. Vui lòng đến cơ sở đúng lịch hẹn</p>",
                Timestamp = fiveAM,
                Type = NotificationType.Remind
            };

            var notification2 = new Notification
            {
                Data = bookingId.ToString(),
                IsUnread = true,
                Description = $"<p>Sắp tới hẹn lịch khám <span class='n-bold'>{code}</span> lúc <span class='n-bold'>{startTime:hh\\:mm}</span>. Vui lòng đến cơ sở đúng lịch hẹn</p>",
                Timestamp = notiDateTime1,
                Type = NotificationType.Remind
            };

            var ownerId = _executionContext.Identity;

            if (timeUntilAppointment > TimeSpan.Zero)
            {
                BackgroundJob.Schedule<ICustomerWriteRepository>(
                    svc => svc.AddNotificationJobAsync(notification2, ownerId),
                    timeUntilAppointment);
            }

            if (timeUntilFiveAM > TimeSpan.Zero)
            {
                BackgroundJob.Schedule<ICustomerWriteRepository>(
                    svc => svc.AddNotificationJobAsync(notification, ownerId),
                    timeUntilFiveAM);
            }
            return Task.CompletedTask;
        }

        public void TestSchedule()
        {
            DateTime now = DateTime.Now;
            TimeSpan timeUntilAppointment = TimeSpan.FromMinutes(1);

            var notification = new Notification
            {
                Data = "0",
                IsUnread = true,
                Description = $"<p>Test noti</p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.Remind
            };

            var ownerId = _executionContext.Identity;

            if (timeUntilAppointment > TimeSpan.Zero)
            {
                BackgroundJob.Schedule<ICustomerWriteRepository>(
                    svc => svc.AddNotificationJobAsync(notification, ownerId),
                    timeUntilAppointment);
            }
        }
    }
}
