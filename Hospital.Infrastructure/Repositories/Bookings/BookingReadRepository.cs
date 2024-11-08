using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Bookings.Models;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
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
        public async Task<int> GetMaxOrderAsync(long serviceId, DateTime date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken)
        {
            var key = BaseCacheKeys.GetQueueOrder(serviceId, date, start, end);

            var valueFactory = async () =>
            {
                var bookings = await _dbSet
                    .Where(x => x.ServiceId == serviceId && x.Date == date
                    && x.ServiceStartTime == start && x.ServiceEndTime == end)
                    .ToListAsync(cancellationToken);
                if (bookings == null)
                {
                    return 0;
                }
                return bookings.Max(x => x.Order);
            };

            return await _redisCache.GetOrSetAsync(key, valueFactory, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }


        public async Task<int> GetCurrentAsync(long serviceId, CancellationToken cancellationToken)
        {
            var cacheKey = BaseCacheKeys.GetCurrentOrder(serviceId);
            var now = _dateService.GetClientTime();
            var valueFactory = () => _dbSet
                .Where(b => b.ServiceId == serviceId &&
                            b.Date == now.Date &&
                            b.ServiceStartTime <= now.TimeOfDay &&
                            b.ServiceEndTime >= now.TimeOfDay &&
                            b.Status == BookingStatus.Doing)
                .Select(b => b.Order)
                .SingleOrDefaultAsync();
            return await _redisCache.GetOrSetAsync<int>(cacheKey, valueFactory, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
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

            var guardExpression = GuardDataAccess(spec, ignoreBranch: true).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Code)
                         .ThenByDescending(x => x.Date)
                         .ThenByDescending(x => x.Modified ?? x.Created);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<Booking>(data, count);
        }

        public async Task<PagingResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long ownerId = 0, bool ignoreOwner = false, CancellationToken cancellationToken = default)
        {
            ISpecification<Booking> spec = new GetBookingsByStatusSpecification(status);
            if (ownerId > 0)
            {
                spec = spec.And(new GetBookingsByHealthProfileIdSpecification(ownerId));
            }

            if (date != default)
            {
                spec = spec.And(new GetBookingsByDateSpecification(date));
            }

            if (excludeId > 0)
            {
                spec = spec.Not(new GetByIdSpecification<Booking>(excludeId));
            }

            var guardExpression = GuardDataAccess(spec, ignoreOwner).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Code)
                         .ThenByDescending(x => x.Date)
                         .ThenByDescending(x => x.Modified ?? x.Created);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
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
    }
}
