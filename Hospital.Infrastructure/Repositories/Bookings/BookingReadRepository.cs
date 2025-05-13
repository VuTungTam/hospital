using Hospital.Application.Models.Bookings;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
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

            if (_executionContext.AccountType == AccountType.Customer || _executionContext.IsSA)
            {
                return spec;
            }

            else
            {
                if (_executionContext.AccountType == AccountType.Doctor)
                {
                    spec = spec.And(new LimitByDoctorIdSpecification<Booking>(_executionContext.Identity));
                }
                spec = spec.And(new LimitByFacilityIdSpecification<Booking>(_executionContext.FacilityId));
                if (_executionContext.ZoneId > 0)
                {
                    spec = spec.And(new LimitByZoneIdSpecification<Booking>(_executionContext.ZoneId));
                }
            }

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

            var data = await _redisCache.GetAsync<int>(cacheEntry.Key, cancellationToken);

            if (data == 0)
            {
                var bookings = await _dbSet.AsNoTracking()
                    .Where(x => x.ServiceId == serviceId && x.Date == date
                    && x.TimeSlotId == timeSlotId && x.Status == BookingStatus.Confirmed)
                    .ToListAsync(cancellationToken);
                if (bookings.Any())
                {
                    data = bookings.Max(x => x.Order);
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
                }
                else
                {
                    data = 0;
                }
            }
            return data;
        }
        public async Task<CurrentBookingModel> GetCurrentAsync(long serviceId, long timeSlotId, CancellationToken cancellationToken)
        {
            var defaultModel = new CurrentBookingModel
            {
                BookingId = "0",
                Order = "0"
            };
            //var now = _dateService.GetClientTime();

            var now = new DateTime(2025, 05, 20);
            var cacheEntry = CacheManager.GetCurrentOrderCacheEntry(serviceId, now, timeSlotId);

            var cachedData = await _redisCache.GetAsync<CurrentBookingModel>(cacheEntry.Key, cancellationToken);

            if (int.TryParse(cachedData?.Order, out var order) && order > 0)
            {
                return cachedData;
            }

            var spec = new GetBookingsByDateSpecification(now)
                .And(new GetBookingsByServiceIdSpecification(serviceId))
                .And(new GetBookingsByTimeSlotSpecification(timeSlotId));

            var option = new QueryOption { IgnoreOwner = true };
            var bookings = await GetBySpecificationAsync(spec, option, cancellationToken);

            var currentBooking = bookings.FirstOrDefault(b => b.Status == BookingStatus.Doing)
                              ?? bookings.Where(b => b.Status == BookingStatus.Confirmed)
                                         .OrderBy(b => b.Order)
                                         .FirstOrDefault();

            if (currentBooking == null)
            {
                return defaultModel;
            }

            var data = new CurrentBookingModel
            {
                BookingId = currentBooking.Id.ToString(),
                Order = currentBooking.Order.ToString()
            };

            await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken);

            return data;
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

        public async Task<PaginationResult<Booking>> GetDoctorPagingWithFilterAsync(Pagination pagination, BookingStatus status,
        long serviceId, long timeSlotId, DateTime date = default, long ownerId = 0, CancellationToken cancellationToken = default)
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

            if (serviceId > 0)
            {
                spec = spec.And(new GetBookingsByServiceIdSpecification(serviceId));
            }

            if (timeSlotId > 0)
            {
                spec = spec.And(new GetBookingsByTimeSlotSpecification(timeSlotId));
            }

            var option = new QueryOption
            {
                IgnoreOwner = true,
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

        public async Task<PaginationResult<Booking>> GetBookingInQueueAsync(Pagination pagination, long serviceId, long timeSlotId, DateTime date = default, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingByQueueStatusSpecification();

            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (serviceId > 0)
            {
                spec = spec.And(new GetBookingsByServiceIdSpecification(serviceId));
            }

            if (timeSlotId > 0)
            {
                spec = spec.And(new GetBookingsByTimeSlotSpecification(timeSlotId));
            }

            var option = new QueryOption
            {
                IgnoreOwner = true,
            };
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderBy(x => x.Order);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<Booking>(data, count);
        }
    }
}
