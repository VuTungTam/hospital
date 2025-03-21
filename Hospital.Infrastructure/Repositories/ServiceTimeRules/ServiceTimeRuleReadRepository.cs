using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthServices;
using Hospital.Domain.Specifications.ServiceTimeRules;
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
using Hospital.SharedKernel.Specifications;
using Hospital.Domain.Specifications.Customers;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;

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

        public async Task<PaginationResult<ServiceTimeRule>> GetPagingWithFilterAsync(Pagination pagination, long serviceId, DayOfWeek? dayOfWeek, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            var spec = new GetServiceTimeRuleByServiceIdSpecification(serviceId);

            if(dayOfWeek.HasValue)
            {
                spec.And(new GetServiceTimeRuleByDayOfWeekSpecification(dayOfWeek.Value));
            }

            query = query.Where(spec.GetExpression());

            var count = await query.CountAsync(cancellationToken);

            query = query.Skip(pagination.Offset)
                         .Take(pagination.Size)
                         .BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);

            return new PaginationResult<ServiceTimeRule>(data, count);
        }
    }
}
