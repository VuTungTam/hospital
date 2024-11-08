using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Newses;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Newes
{
    public class NewsWriteRepository : WriteRepository<News>, INewsWriteRepository
    {
        public NewsWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override async Task UpdateAsync(News entity, List<string> specificUpdates = null, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, specificUpdates, cancellationToken);

            var key = AppCacheKeys.GetNewsBySlugKey(entity.Slug);
            await _redisCache.RemoveAsync(key, cancellationToken);
        }

        public override async Task DeleteAsync(IEnumerable<News> entities, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(entities, cancellationToken);

            var tasks = new List<Task>();
            foreach (var entity in entities)
            {
                var key = AppCacheKeys.GetNewsBySlugKey(entity.Slug);
                tasks.Add(_redisCache.RemoveAsync(key, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }
    }
}
