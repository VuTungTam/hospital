using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Domain.Specifications.ServiceTimeRules;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using Hospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.ServiceTimeRules
{
    public class ServiceTimeRuleReadRepository : ReadRepository<ServiceTimeRule>, IServiceTimeRuleReadRepository
    {
        public ServiceTimeRuleReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<int> GetMaxSlotAsync(long serviceId, DateTime date, CancellationToken cancellationToken)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;

            var rule = await _dbSet.FirstOrDefaultAsync(x => x.DayOfWeek == dayOfWeek && x.ServiceId == serviceId);

            if (rule == null) {
                throw new BadRequestException("Khung gio khong phu hop");
            }

            return rule.MaxPatients;
        }

        public async Task<PaginationResult<ServiceTimeRule>> GetPagingWithFilterAsync(Pagination pagination,long? serviceId, DayOfWeek? dayOfWeek, CancellationToken cancellationToken = default)
        {
            ISpecification<ServiceTimeRule> spec = null; 
                
            if (serviceId.HasValue && serviceId.Value > 0)
            {
                spec = spec.And(new GetServiceTimeRuleByServiceIdSpecification(serviceId.Value));
            }

            if (dayOfWeek.HasValue && dayOfWeek.Value > 0)
            {
                spec = spec.And(new GetServiceTimeRuleByDayOfWeekSpecification(dayOfWeek.Value));
            }
            QueryOption option = new QueryOption();
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<ServiceTimeRule>(data, count);
        }
    }
}
