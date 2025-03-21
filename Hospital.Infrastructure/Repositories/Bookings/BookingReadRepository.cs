using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Hospital.Infrastructure.Extensions;
using Hospital.SharedKernel.Infrastructure.Caching.Models;

namespace Hospital.Infrastructure.Repositories.Bookings
{
    public class BookingReadRepository : ReadRepository<Booking>, IBookingReadRepository
    {
        private readonly IDateService _dateService;
        public BookingReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IDateService dateService
        ) : base(serviceProvider, localizer, redisCache)
        {
            _dateService = dateService;
        }

        public async Task<int> GetMaxOrderAsync(long serviceId, DateTime date, long timeSlotId , CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(serviceId, date, timeSlotId);

            var valueFactory = async () =>
            {
                var bookings = await _dbSet.AsNoTracking()
                    .Where(x => x.ServiceId == serviceId && x.Date == date
                    && x.TimeSlotId == timeSlotId)
                    .ToListAsync(cancellationToken);
                if (bookings == null)
                {
                    return 0;
                }
                return bookings.Max(x => x.Order);
            };
            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }


        public async Task<int> GetCurrentAsync(long serviceId, long timeSlotId, CancellationToken cancellationToken)
        {
            var now = _dateService.GetClientTime();

            var cacheKey = CacheManager.GetCurrentOrderCacheEntry(serviceId, now, timeSlotId);

            var spec = new GetBookingsByDateSpecification(now);

            spec.And(new GetBookingsByServiceIdSpecification(serviceId));

            spec.And(new GetBookingsByTimeSlotSpecification(timeSlotId));

            spec.And(new GetBookingsByStatusSpecification(BookingStatus.Doing));

            QueryOption option = new QueryOption
            {
                IgnoreOwner = true,
            };
            
            var booking = await FindBySpecificationAsync(spec, option, cancellationToken);

            async Task<int> valueFactory()
            {
                await FindBySpecificationAsync(spec, option, cancellationToken);
                return booking.Order;
            }

            return await _redisCache.GetOrSetAsync<int>(cacheKey.Key, valueFactory, TimeSpan.FromSeconds(cacheKey.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }
        public async Task<PaginationResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceId = 0, DateTime date = default, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingsByStatusSpecification(status);
            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (serviceId > 0)
            {
                spec = spec.And(new GetBookingsByServiceIdSpecification(serviceId));
            }
            QueryOption option = new QueryOption
            {
                IgnoreOwner = false,
                Includes = new string[] { nameof(Booking.BookingSymptoms) }
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking().IncludesRelateData(option.Includes), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Code)
                         .ThenByDescending(x => x.Date)
                         .ThenByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);
            
            return new PaginationResult<Booking>(data, count);
        }

        public async Task<PaginationResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long profileId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingsByStatusSpecification(status);
            if (profileId > 0)
            {
                spec = spec.And(new GetBookingsByHealthProfileIdSpecification(profileId));
            }

            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (excludeId > 0)
            {
                spec = spec.Not(new GetByIdSpecification<Booking>(excludeId));
            }
            QueryOption option = new QueryOption
            {
                IgnoreOwner = true,
                Includes = new string[] { nameof(Booking.BookingSymptoms) }
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking().IncludesRelateData(option.Includes), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Code)
                         .ThenByDescending(x => x.Date)
                         .ThenByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<Booking>(data, count);
        }

    //    public async Task<int> GetBookingQuantity(
    //long? serviceId, DateTime? date, TimeSpan? start, TimeSpan? end, List<BookingStatus>? status, CancellationToken cancellationToken)
    //    {
    //        var specs = new List<ISpecification<Booking>>();

    //        if (status?.Any() == true)
    //        {
    //            foreach (var item in status)
    //            {
    //                specs.Add(new GetBookingsByStatusSpecification(item));
    //            }
    //        }

    //        if (serviceId.HasValue && serviceId.Value > 0)
    //        {
    //            specs.Add(new GetBookingsByServiceIdSpecification(serviceId.Value));
    //        }

    //        if (date.HasValue)
    //        {
    //            specs.Add(new GetBookingsByDateSpecification(date.Value));
    //        }

    //        if (start.HasValue && end.HasValue)
    //        {
    //            specs.Add(new GetBookingsByTimeSlotSpecification(start.Value, end.Value));
    //        }

    //        var combinedSpec = specs.Aggregate((spec1, spec2) => spec1.And(spec2));
    //        QueryOption option = new QueryOption();
    //        var guardExpression = GuardDataAccess(combinedSpec,option).GetExpression();

    //        return await _dbSet.AsNoTracking()
    //                           .Where(guardExpression)
    //                           .CountAsync(cancellationToken);
    //    }

    }
}
