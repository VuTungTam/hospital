using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Articles;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Articles
{
    public class ArticleWriteRepository : WriteRepository<Article>, IArticleWriteRepository
    {
        public ArticleWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override async Task UpdateAsync(Article entity, List<string> excludes = null, CancellationToken cancellationToken = default)
        {
            excludes = new List<string> { nameof(Article.ViewCount) };

            await base.UpdateAsync(entity, excludes, cancellationToken);

            var cacheEntry = AppCacheManager.GetAricleBySlugCacheEntry(entity.Slug);
            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
        }

        public override async Task DeleteAsync(IEnumerable<Article> entities, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(entities, cancellationToken);

            var tasks = new List<Task>();
            foreach (var entity in entities)
            {
                var cacheEntry = AppCacheManager.GetAricleBySlugCacheEntry(entity.Slug);
                tasks.Add(_redisCache.RemoveAsync(cacheEntry.Key, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        public async Task IncreaseViewCountAsync(long id, CancellationToken cancellationToken)
        {
            var sql = $"UPDATE {new Article().GetTableName()} SET ViewCount = ViewCount + 1 WHERE Id = {id} AND IsDeleted = 0";

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
