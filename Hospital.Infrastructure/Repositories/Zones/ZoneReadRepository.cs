using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Specifications;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Zones
{
    public class ZoneReadRepository : ReadRepository<Zone>, IZoneReadRepository
    {
        public ZoneReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<Zone> GetZoneBySpecialtyId(long specialtyId, long facilityId, CancellationToken cancellationToken)
        {
            var zone = await _dbContext.Zones
                .Where(z => z.FacilityId == facilityId &&
                            z.ZoneSpecialties.Any(zs => zs.SpecialtyId == specialtyId))
                .FirstOrDefaultAsync(cancellationToken);
            return zone;
        }

        public async Task<List<Zone>> GetZonesByFacilityId(long facilityId, CancellationToken cancellationToken)
        {
            var zone = await _dbContext.Zones
                .Where(z => z.FacilityId == facilityId)
                .Include(x => x.ZoneSpecialties)
                .ToListAsync(cancellationToken);
            return zone;
        }

        public override ISpecification<Zone> GuardDataAccess<Zone>(ISpecification<Zone> spec, QueryOption option = default)
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<Zone>(x => true);

            spec = spec.And(base.GuardDataAccess(spec, option));

            if (_executionContext.AccountType == AccountType.Customer)
            {
                return spec;
            }
            else
            {
                spec = spec.And(new LimitByFacilityIdSpecification<Zone>(_executionContext.FacilityId));
            }
            return spec;
        }
    }
}
