using Hospital.Domain.Constants;
using Hospital.Infra.EFConfigurations;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories.AppConfigs
{
    public class SystemConfigurationReadRepository : ISystemConfigurationReadRepository
    {
        private readonly IRedisCache _redisCache;
        private readonly AppDbContext _dbContext;
        private readonly IExecutionContext _executionContext;

        public SystemConfigurationReadRepository(
            IRedisCache redisCache,
            AppDbContext dbContext,
            IExecutionContext executionContext
        )
        {
            _redisCache = redisCache;
            _dbContext = dbContext;
            _executionContext = executionContext;
        }

        public async Task<SystemConfiguration> GetAsync(CancellationToken cancellationToken = default)
        {
            var key = BaseCacheKeys.GetSystemConfigurationKey();
            var valueFactory = () => _dbContext.SystemConfigurations.FirstOrDefaultAsync(cancellationToken);

            var config = await _redisCache.GetOrSetAsync(key, valueFactory, TimeSpan.FromSeconds(AppCacheTime.SystemConfiguration), cancellationToken: cancellationToken);
            if (config == null)
            {
                throw new CatchableException("Not found app config");
            }

            return config;
        }
    }
}
