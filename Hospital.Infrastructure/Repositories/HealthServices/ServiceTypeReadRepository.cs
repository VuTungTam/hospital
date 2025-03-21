using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class ServiceTypeReadRepository : ReadRepository<ServiceType>, IServiceTypeReadRepository
    {
        public ServiceTypeReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
