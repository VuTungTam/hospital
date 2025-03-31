using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.HealthProfiles;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthProfiles
{
    public class HealthProfileReadRepository : ReadRepository<HealthProfile>, IHealthProfileReadRepository
    {
        public HealthProfileReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public Task<HealthProfile> GetProfileById(long id, CancellationToken cancellationToken)
        {
            ISpecification<HealthProfile> spec = new IdEqualsSpecification<HealthProfile>(id);

            if (_executionContext.AccountType == AccountType.Customer)
            {
                spec = spec.And(new LimitByOwnerIdSpecification<HealthProfile>(_executionContext.Identity));
            }
            else
            {
                spec = spec.And(new ExpressionSpecification<HealthProfile>(x => x.Bookings.Any(bk => bk.FacilityId == _executionContext.FacilityId)));
            }

            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }


        public async Task<PaginationResult<HealthProfile>> GetPagingWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> spec = null;

            if (userId > 0) {

                spec = new GetHealthProfileByOwnerIdSpecification(userId);

            }
            QueryOption option = new QueryOption
            {
                IgnoreOwner = true,
            };

            spec = spec.And(new ExpressionSpecification<HealthProfile>(x => x.Bookings.Any(bk => bk.FacilityId == _executionContext.FacilityId)));

            var guardExpression = GuardDataAccess(spec, option).GetExpression();

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthProfile>(data, count);
        }
    }
}
