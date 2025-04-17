using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Metas
{
    public class ScriptWriteRepository : WriteRepository<Script>, IScriptWriteRepository
    {
        public ScriptWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override async Task UpdateAsync(Script entity, List<string> excludes = null, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, excludes, cancellationToken);

            await _redisCache.RemoveAsync(AppCacheManager.Script.Key, cancellationToken);
        }
    }
}
