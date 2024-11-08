using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hospital.Infrastructure.Repositories.ServiceTimeRules
{
    public class ServiceTimeRuleWriteRepository : WriteRepository<ServiceTimeRule>, IServiceTimeRuleWriteRepository
    {
        public ServiceTimeRuleWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
            
        }

        
    }
}
