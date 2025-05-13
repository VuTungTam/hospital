using System.Security.Cryptography.X509Certificates;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.HealthProfiles;
using Hospital.Infrastructure.EFConfigurations;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
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
        public HealthProfileReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
             IRedisCache redisCache
             ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PaginationResult<HealthProfile>> GetPagingWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> spec = new ExpressionSpecification<HealthProfile>(x => true);

            if (userId > 0)
            {
                spec = spec.And(new GetHealthProfileByOwnerIdSpecification(userId));
            }

            var option = new QueryOption
            {
                IgnoreOwner = true
            };

            var guardExpression = GuardDataAccess(spec, option).GetExpression();

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);

            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthProfile>(data, count);
        }

        public async Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> specProfile = new HealthProfileByPhoneSpecification(phone);

            if (exceptId > 0)
            {
                specProfile = specProfile.Not(new IdEqualsSpecification<HealthProfile>(exceptId));
            }

            return await _dbContext.HealthProfiles.AnyAsync(specProfile.GetExpression(), cancellationToken);
        }

        public async Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> specProfile = new HealthProfileByEmailSpecification(email);

            if (exceptId > 0)
            {
                specProfile = specProfile.Not(new IdEqualsSpecification<HealthProfile>(exceptId));
            }

            return await _dbContext.HealthProfiles.AnyAsync(specProfile.GetExpression(), cancellationToken);
        }

        public async Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthProfile> specProfile = new HealthProfileByCodeSpecification(code);

            if (exceptId > 0)
            {
                specProfile = specProfile.Not(new IdEqualsSpecification<HealthProfile>(exceptId));
            }

            return await _dbContext.HealthProfiles.AnyAsync(specProfile.GetExpression(), cancellationToken);
        }

    }
}
