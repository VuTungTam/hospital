using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Newes;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.Newes
{
    public class NewsReadRepository : ReadRepository<News>, INewsReadRepository
    {
        public NewsReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<News> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var key = AppCacheKeys.GetNewsBySlugKey(slug);
            var data = await _redisCache.GetAsync<News>(key, cancellationToken);
            if (data == null)
            {
                QueryOption option = new QueryOption();
                data = await FindBySpecificationAsync(new GetNewsBySlugSpecification(slug), option, cancellationToken: cancellationToken);
                if (data != null)
                {
                    await _redisCache.SetAsync(key, data, TimeSpan.FromSeconds(AppCacheTime.NewsSlug), cancellationToken: cancellationToken);
                }
            }

            return data;
        }

        public async Task<PagingResult<News>> GetPagingWithFilterAsync(Pagination pagination, NewsStatus status, long excludeId = 0, DateTime postDate = default, bool clientSort = false, CancellationToken cancellationToken = default)
        {
            ISpecification<News> spec = new GetNewsByStatusSpecification(status)
                           .And(new GetNewsByPostDateSpecification(postDate));

            if (excludeId > 0)
            {
                spec = spec.Not(new GetByIdSpecification<News>(excludeId));
            }
            QueryOption option = new QueryOption();
            var guardExpression = GuardDataAccess(spec,option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            if (true || clientSort)
            {
                query = query.Where(guardExpression)
                             .OrderByDescending(x => x.IsHighlight)
                             .ThenByDescending(x => x.PostDate)
                             .ThenByDescending(x => x.Modified ?? x.Created);
            }
            //else
            //{
            //    query = query.Where(guardExpression)
            //                 .BuildOrderBy(pagination.Sorts);
            //}

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<News>(data, count);
        }
    }
}
