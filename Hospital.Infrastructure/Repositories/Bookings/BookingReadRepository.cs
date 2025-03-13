using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Infra.Repositories;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VetHospital.Infrastructure.Extensions;

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

        public virtual async Task<Booking> GetBookingByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                   .AsNoTracking()
                   .Include(x => x.BookingSymptoms)
                   .ThenInclude(x => x.Symptom)
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> GetMaxOrderAsync(long serviceId, DateTime date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken)
        {
            var key = BaseCacheKeys.GetQueueOrder(serviceId, date, start, end);

            var valueFactory = async () =>
            {
                var bookings = await _dbSet.AsNoTracking()
                    .Where(x => x.ServiceId == serviceId && x.Date == date
                    && x.ServiceStartTime == start && x.ServiceEndTime == end)
                    .ToListAsync(cancellationToken);
                if (bookings == null)
                {
                    return 0;
                }
                return bookings.Max(x => x.Order);
            };
            return await valueFactory();
            //return await _redisCache.GetOrSetAsync(key, valueFactory, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }


        public async Task<int> GetCurrentAsync(long serviceId, CancellationToken cancellationToken)
        {
            var cacheKey = BaseCacheKeys.GetCurrentOrder(serviceId);
            var now = _dateService.GetClientTime();
            var spec = new GetBookingsByDateSpecification(now);
            spec.And(new GetBookingsByServiceIdSpecification(serviceId));
            spec.And(new GetBookingsByStatusSpecification(BookingStatus.Doing));

            QueryOption option = new QueryOption
            {
                Includes = null,
                IgnoreOwner = true,
            };
            var booking =await FindBySpecificationAsync(spec, option, cancellationToken);
            //var valueFactory = () => _dbSet
            //    .Where(b => b.ServiceId == serviceId &&
            //                b.Date == now.Date &&
            //                b.ServiceStartTime <= now.TimeOfDay &&
            //                b.ServiceEndTime >= now.TimeOfDay &&
            //                b.Status == BookingStatus.Doing)
            //    .Select(b => b.Order)
            //    .SingleOrDefaultAsync();
            //return await _redisCache.GetOrSetAsync<int>(cacheKey, valueFactory, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
            return booking.Order;         
        }
        public async Task<PagingResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceId = 0, DateTime date = default, CancellationToken cancellationToken = default)
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
                         .ThenByDescending(x => x.Modified ?? x.Created);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);
            
            return new PagingResult<Booking>(data, count);
        }

        public async Task<PagingResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long profileId = 0, CancellationToken cancellationToken = default)
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
                         .ThenByDescending(x => x.Modified ?? x.Created);

            var data = await query
                .BuildLimit(pagination.Offset, pagination.Size).ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<Booking>(data, count);
        }

        public async Task<List<Booking>> GetNextBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet
                .Where(x => x.Date == booking.Date && x.ServiceId == booking.ServiceId
                && x.ServiceStartTime == booking.ServiceStartTime 
                && x.ServiceEndTime == booking.ServiceEndTime 
                && x.Order > booking.Order).ToListAsync();
            return result;
        }
        public async Task<int> GetBookingQuantity(
    long? serviceId, DateTime? date, TimeSpan? start, TimeSpan? end, List<BookingStatus>? status, CancellationToken cancellationToken)
        {
            var specs = new List<ISpecification<Booking>>();

            if (status?.Any() == true)
            {
                foreach (var item in status)
                {
                    specs.Add(new GetBookingsByStatusSpecification(item));
                }
            }

            if (serviceId.HasValue && serviceId.Value > 0)
            {
                specs.Add(new GetBookingsByServiceIdSpecification(serviceId.Value));
            }

            if (date.HasValue)
            {
                specs.Add(new GetBookingsByDateSpecification(date.Value));
            }

            if (start.HasValue && end.HasValue)
            {
                specs.Add(new GetBookingsByTimeRangeSpecification(start.Value, end.Value));
            }

            var combinedSpec = specs.Aggregate((spec1, spec2) => spec1.And(spec2));
            QueryOption option = new QueryOption();
            var guardExpression = GuardDataAccess(combinedSpec,option).GetExpression();

            return await _dbSet.AsNoTracking()
                               .Where(guardExpression)
                               .CountAsync(cancellationToken);
        }

    }
}
