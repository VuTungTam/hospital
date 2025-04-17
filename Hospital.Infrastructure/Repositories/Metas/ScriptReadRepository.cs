using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Metas
{
    public class ScriptReadRepository : ReadRepository<Script>, IScriptReadRepository
    {
        public ScriptReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<Script> ReadAsync(CancellationToken cancellationToken)
        {
            var cacheEntry = AppCacheManager.Script;
            var valueFactory = () => _dbSet.FirstOrDefaultAsync(cancellationToken);

            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }
    }
}
