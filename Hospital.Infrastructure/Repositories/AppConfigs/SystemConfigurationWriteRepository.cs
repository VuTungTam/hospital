using Hospital.Infrastructure.EFConfigurations;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;

namespace Hospital.Infrastructure.Repositories.AppConfigs
{
    public class SystemConfigurationWriteRepository : ISystemConfigurationWriteRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IRedisCache _redisCache;

        public SystemConfigurationWriteRepository(AppDbContext dbContext, IRedisCache redisCache)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
        }

        public async Task SaveAsync(SystemConfiguration config, CancellationToken cancellationToken)
        {
            _dbContext.SystemConfigurations.Update(config);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);

            var cacheEntry = CacheManager.SystemConfiguration;
            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
        }
    }
}
