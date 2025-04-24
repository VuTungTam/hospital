using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceWriteRepository : WriteRepository<HealthService>, IHealthServiceWriteRepository
    {
        public HealthServiceWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task UpdateServiceAsync(HealthService oldEntity, HealthService newEntity, CancellationToken cancellationToken)
        {


            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
