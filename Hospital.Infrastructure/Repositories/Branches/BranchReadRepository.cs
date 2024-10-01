using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Domain.Constants;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Branches;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.Branches
{
    public class BranchReadRepository : ReadRepository<Branch>, IBranchReadRepository
    {
        public BranchReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async override Task<List<Branch>> GetAsync(string[] includes = null, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default)
        {
            var key = GetCacheKey(type: "all");
            var data = await _redisCache.GetAsync<List<Branch>>(key, cancellationToken: cancellationToken);
            if (data == null)
            {
                data = await _dbSet.OrderBy(x => x.Id).ToListAsync(cancellationToken);
                if (data != null && data.Any())
                {
                    await _redisCache.SetAsync(key, data, TimeSpan.FromSeconds(AppCacheTime.Records), cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public async Task<PagingResult<Branch>> GetPagingWithFilterAsync(Pagination pagination, BranchStatus status, CancellationToken cancellationToken)
        {
            ISpecification<Branch> spec = new GetBranchesByStatusSpecification(status);

            var guardExpression = GuardDataAccess(spec).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Active)
                         .ThenByDescending(x => x.Modified ?? x.Created);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<Branch>(data, count);
        }
    }
}
