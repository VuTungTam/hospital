using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Domain.Specifications.Auths;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Auth
{
    public class LoginHistoryReadRepository : ReadRepository<LoginHistory>, ILoginHistoryReadRepository
    {
        public LoginHistoryReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PaginationResult<LoginHistory>> GetPaginationWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default)
        {
            var spec = new LoginHistoryByUserIdEqualsSpecification(userId);
            var option = new QueryOption { IgnoreOwner = true};
            var guardExpression = GuardDataAccess(spec, option).GetExpression();

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.Timestamp);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<LoginHistory>(data, count);
        }
    }
}
