using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

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

        public override ISpecification<Booking> GuardDataAccess<Booking>(ISpecification<Booking> spec, QueryOption option = default)
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<Booking>(x => true);

            spec = spec.And(base.GuardDataAccess(spec, option));

            // if (!option.IgnoreDoctor)
            // {
            //     spec = spec.And(new LimitByDoctorIdSpecification<Booking>(_executionContext.Identity));

            // }
            return spec;
        }

        public override async Task<Booking> GetByIdAsync(long id, QueryOption option = null, CancellationToken cancellationToken = default)
        {
            var cacheEntry = GetCacheEntry(id);
            var data = await _redisCache.GetAsync<Booking>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                ISpecification<Booking> spec = new IdEqualsSpecification<Booking>(id);

                if (_executionContext.AccountType == AccountType.Customer)
                {
                    spec = spec.And(new LimitByOwnerIdSpecification<Booking>(_executionContext.Identity));
                }
                data = await _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);

                if (data != null)
                {
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(AppCacheTime.RecordWithId), cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public async Task<int> GetMaxOrderAsync(long serviceId, DateTime date, long timeSlotId, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(serviceId, date, timeSlotId);

            async Task<int> valueFactory()
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
            }
            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }

        public async Task<int> GetCurrentAsync(long serviceId, long timeSlotId, CancellationToken cancellationToken)
        {
            var now = _dateService.GetClientTime();

            var cacheKey = CacheManager.GetCurrentOrderCacheEntry(serviceId, now, timeSlotId);
            async Task<int> ValueFactory()
            {
                QueryOption option = new() { IgnoreOwner = true };

                var spec = new GetBookingsByDateSpecification(now)
                    .And(new GetBookingsByServiceIdSpecification(serviceId))
                    .And(new GetBookingsByTimeSlotSpecification(timeSlotId));

                var bookings = await GetBySpecificationAsync(spec, option, cancellationToken);

                var currentBooking = bookings.FirstOrDefault(b => b.Status == BookingStatus.Doing);

                currentBooking ??= bookings.Where(b => b.Status == BookingStatus.Confirmed)
                                           .OrderBy(b => b.Order)
                                           .FirstOrDefault();

                if (currentBooking == null)
                    throw new BadRequestException("Khong the lay so thu tu hien tai");

                return currentBooking.Order;
            }

            return await _redisCache.GetOrSetAsync(cacheKey.Key, ValueFactory,
                TimeSpan.FromSeconds(cacheKey.ExpiriesInSeconds), cancellationToken);
        }
        public async Task<PaginationResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceTypeId = 0, DateTime date = default, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingsByStatusSpecification(status);
            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (serviceTypeId > 0)
            {
                spec = spec.And(new GetBookingsByServiceTypeIdSpecification(serviceTypeId));
            }
            var option = new QueryOption
            {
                IgnoreOwner = false,
                IgnoreDoctor = true,
                IgnoreFacility = true,
                IgnoreZone = true,
                Includes = new string[] { nameof(Booking.Service) }
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderBy(x => x.Date)
                         .ThenBy(x => x.TimeSlotId)
                         .ThenBy(x => x.Status == BookingStatus.Completed ? 1 : 0)
                         .ThenBy(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<Booking>(data, count);
        }

        public async Task<PaginationResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long ownerId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingsByStatusSpecification(status);
            if (ownerId > 0)
            {
                spec = spec.And(new OwnerIdEqualsSpecification<Booking>(ownerId));
            }

            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (excludeId > 0)
            {
                spec = spec.Not(new GetByIdSpecification<Booking>(excludeId));
            }
            var option = new QueryOption
            {
                IgnoreOwner = true,
                IgnoreDoctor = true,
                IgnoreFacility = false,
                IgnoreZone = true,
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderBy(x => x.Date)
                         .ThenBy(x => x.TimeSlotId)
                         .ThenBy(x => x.ModifiedAt)
                         .ThenBy(x => x.ServiceId)
                         .ThenBy(x => x.Status);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<Booking>(data, count);
        }

        public async Task<Booking> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetBookingIdByCodeCacheEntry(code);
            long masterId = await _redisCache.GetAsync<long>(cacheEntry.Key, cancellationToken);
            Booking data;
            if (masterId != 0)
            {
                data = await GetByIdAsync(masterId, cancellationToken: cancellationToken);
            }
            else
            {
                data = await GetBookingFromDatabaseAsync(code, cancellationToken);
                if (data == null)
                {
                    throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
                }
                else
                {
                    var cacheEntry2 = GetCacheEntry(data.Id);
                    await _redisCache.SetAsync(cacheEntry.Key, data.Id, TimeSpan.FromSeconds(AppCacheTime.RecordWithId), cancellationToken: cancellationToken);
                    await _redisCache.SetAsync(cacheEntry2.Key, data, TimeSpan.FromSeconds(AppCacheTime.RecordWithId), cancellationToken: cancellationToken);
                }
            }

            return data;
        }

        private async Task<Booking> GetBookingFromDatabaseAsync(string code, CancellationToken cancellationToken)
        {
            var sql = $@"
                SELECT * FROM {new Booking().GetTableName()} 
                WHERE Code = @code AND OwnerId = @ownerId";

            var parameters = new[]
            {
                new SqlParameter("@code", code),
                new SqlParameter("@ownerId", _executionContext.Identity)
            };

            return await _dbSet.FromSqlRaw(sql, parameters)
                               .IgnoreQueryFilters()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<Booking>> GetBookingsToReorder(Booking cancelBooking, CancellationToken cancellationToken)
        {
            var data = await _dbSet.AsNoTracking()
            .Where(x => x.Date == cancelBooking.Date && x.ServiceId == cancelBooking.ServiceId
                && x.TimeSlotId == cancelBooking.TimeSlotId && x.Order > cancelBooking.Order).ToListAsync(cancellationToken);
            return data;
        }
    }
}
