using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;

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

            return rule.MaxPatients;
        }
    }
}
