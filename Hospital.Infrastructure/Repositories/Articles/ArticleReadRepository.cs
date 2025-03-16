using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Articles;
using Hospital.Infra.Repositories;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Articles
{
    public class ArticleReadRepository : ReadRepository<Article>, IArticleReadRepository
    {
        public ArticleReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<bool> IsSlugExistsAsync(long excludeId, string slug, CancellationToken cancellationToken)
        {
            ISpecification<Article> spec = new ArticleBySlugEqualsSpecification(slug);
            if (excludeId > 0)
            {
                spec = spec.Not(new IdEqualsSpecification<Article>(excludeId));
            }

            return await _dbSet.AnyAsync(spec.GetExpression(), cancellationToken);
        }

        public async Task<Article> GetBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken = default)
        {
            var cacheEntry = AppCacheManager.GetAricleBySlugCacheEntry(slug);
            var data = await _redisCache.GetAsync<Article>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                data = await FindBySpecificationAsync(new ArticleBySlugEqualsSpecification(slug), cancellationToken: cancellationToken);
                if (data != null)
                {
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
                }
            }

            if (data != null)
            {
                if (!langs.Contains("vi-VN"))
                {
                    data.Title = "";
                    data.Toc = "";
                    data.Summary = "";
                    data.Content = "";
                }

                if (!langs.Contains("en-US"))
                {
                    data.TitleEn = "";
                    data.TocEn = "";
                    data.SummaryEn = "";
                    data.ContentEn = "";
                }
            }

            return data;
        }

        public Task<int> GetViewCountAsync(long id, CancellationToken cancellationToken = default)
        {
            var spec = new IdEqualsSpecification<Article>(id);
            return _dbSet.FirstOrDefaultAsync(spec.GetExpression(), cancellationToken).Select(x => x.ViewCount);
        }

        public async Task<PaginationResult<Article>> GetPaginationWithFilterAsync(Pagination pagination, ArticleStatus status, long excludeId = 0, DateTime postDate = default, bool clientSort = false, CancellationToken cancellationToken = default)
        {
            ISpecification<Article> spec = new ArticleByStatusEqualsSpecification(status)
                                      .And(new ArticleByPostDateEqualsSpecification(postDate));

            if (excludeId > 0)
            {
                spec = spec.Not(new IdEqualsSpecification<Article>(excludeId));
            }

            var guardExpression = GuardDataAccess(spec).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Where(guardExpression)
                         .OrderByDescending(x => x.IsHighlight)
                         .ThenByDescending(x => x.PostDate)
                         .ThenByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .Select(x => new Article
                                  {
                                      Id = x.Id,
                                      CreatedAt = x.CreatedAt,
                                      CreatedBy = x.CreatedBy,
                                      Image = x.Image,
                                      IsHighlight = x.IsHighlight,
                                      ModifiedAt = x.ModifiedAt,
                                      ModifiedBy = x.ModifiedBy,
                                      PostDate = x.PostDate,
                                      Slug = x.Slug,
                                      Status = x.Status,
                                      Summary = x.Summary,
                                      SummaryEn = x.SummaryEn,
                                      Title = x.Title,
                                      TitleEn = x.TitleEn,
                                      TitleSeo = x.TitleSeo,
                                      Toc = x.Toc,
                                      TocEn = x.TocEn
                                  })
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<Article>(data, count);
        }
    }
}
