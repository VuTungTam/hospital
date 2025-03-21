using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthProfiles
{
    public class HealthProfileWriteRepository : WriteRepository<HealthProfile>, IHealthProfileWriteRepository
    {
        public HealthProfileWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
