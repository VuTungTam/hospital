using AspNetCoreRateLimit;
using Hospital.Application.Dtos.DoctorWorkingContexts;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceReadRepository : ReadRepository<HealthService>, IHealthServiceReadRepository
    {

        private readonly ITimeSlotReadRepository _timeSlotReadRepository;

        public HealthServiceReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            ITimeSlotReadRepository timeSlotReadRepository
            ) : base(serviceProvider, localizer, redisCache)
        {
            _timeSlotReadRepository = timeSlotReadRepository;
        }

        public override ISpecification<HealthService> GuardDataAccess<HealthService>(ISpecification<HealthService> spec, QueryOption option = default)
        {
            option = option ?? new QueryOption();

            spec = base.GuardDataAccess(spec, option);

            // if (!option.IgnoreFacility)
            // {
            //     spec = spec.And(new LimitByFacilityIdSpecification<HealthService>(_executionContext.FacilityId));

            // }
            return spec;
        }
        public async Task<PaginationResult<HealthService>> GetPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long serviceTypeId, long facilityId, long specialtyId, long doctorId, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthService> spec = new GetHealthServicesByStatusSpecification(status);
            if (serviceTypeId > 0)
            {
                spec = spec.And(new GetHealthServicesByTypeSpecification(serviceTypeId));
            }

            if (facilityId > 0)
            {
                spec = spec.And(new GetHealthServicesByFacilityIdSpecification(facilityId));
            }

            if (specialtyId > 0)
            {
                spec = spec.And(new GetHealthServicesBySpecialtyIdSpecification(specialtyId));
            }

            if (doctorId > 0)
            {
                spec = spec.And(new GetHealthServicesByDoctorIdSpecification(doctorId));
            }

            var guardExpression = GuardDataAccess(spec).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                        .Include(s => s.ServiceTimeRules)
                        .Where(guardExpression)
                        .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthService>(data, count);
        }

        public async Task<List<ServiceType>> GetServiceTypeByFacilityIdAsync(long facilityId, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetFacilityServiceType(facilityId);

            var valueFactory = () => _dbContext.HealthServices
                 .Where(s => s.FacilityId == facilityId)
                 .Select(s => s.ServiceType)
                 .Distinct()
                 .ToListAsync(cancellationToken);

            var serviceTypes = await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);

            return serviceTypes;
        }

        public async Task<PaginationResult<HealthService>> GetServiceCurrentAsync(Pagination pagination, long facilityId, long doctorId, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var currentDay = (int)now.DayOfWeek;

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                .Include(s => s.ServiceTimeRules.Where(r => r.DayOfWeek == currentDay))
                .Where(s => s.ServiceTimeRules.Any(r => r.DayOfWeek == currentDay));

            if (facilityId > 0)
                query = query.Where(s => s.FacilityId == facilityId);

            if (doctorId > 0)
                query = query.Where(s => s.DoctorId == doctorId);

            var data = await query.ToListAsync(cancellationToken);
            var filtered = data
                .Where(service => service.ServiceTimeRules.Any(r => IsTimeWithinRule(r, currentTime)))
                .ToList();

            var paged = filtered.Skip(pagination.Offset).Take(pagination.Size).ToList();
            return new PaginationResult<HealthService>(paged, filtered.Count);
        }


        private static bool IsTimeWithinRule(ServiceTimeRule rule, TimeSpan currentTime)
        {
            var inMainRange = currentTime >= rule.StartTime && currentTime <= rule.EndTime;
            var inBreak = rule.StartBreakTime != rule.EndBreakTime &&
                          currentTime >= rule.StartBreakTime && currentTime <= rule.EndBreakTime;

            return inMainRange && !inBreak;
        }

        public async Task<DoctorWorkingContextDto> GetServiceCurrentByDoctorIdAsync(long doctorId, CancellationToken cancellationToken)
        {
            CacheEntry cacheEntry = CacheManager.GetDoctorContext(doctorId);

            var data = await _redisCache.GetAsync<DoctorWorkingContextDto>(cacheEntry.Key, cancellationToken);

            if (data != null)
            {
                return data;
            }

            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var currentDay = (int)now.DayOfWeek;

            var service = await _dbSet.AsNoTracking()
                .Include(s => s.ServiceTimeRules.Where(r => r.DayOfWeek == currentDay))
                .FirstOrDefaultAsync(s =>
                    s.DoctorId == doctorId &&
                    s.ServiceTimeRules.Any(r => r.DayOfWeek == currentDay),
                    cancellationToken);

            if (service == null)
            {
                return null;
            }

            var rule = service.ServiceTimeRules.FirstOrDefault();
            if (rule == null)
            {
                return null;
            }

            var timeSlots = await _timeSlotReadRepository.GetByTimeRuleIdAsync(rule.Id, cancellationToken);

            var currentSlot = timeSlots.FirstOrDefault(ts => ts.Start <= currentTime && ts.End > currentTime);

            if (currentSlot == null)
            {
                return null;
            }

            data = new DoctorWorkingContextDto
            {
                DoctorId = doctorId.ToString(),
                ServiceId = service.Id.ToString(),
                ServiceTimeRuleId = rule.Id.ToString(),
                TimeSlotId = currentSlot.Id.ToString()
            };

            var expired = currentSlot.End - currentTime;
            if (expired.TotalSeconds > 0)
            {
                await _redisCache.SetAsync(cacheEntry.Key, data, expired, cancellationToken);
            }

            return data;

        }
    }
}