using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Specifications;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
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

        public override ISpecification<Zone> GuardDataAccess<Zone>(ISpecification<Zone> spec, QueryOption option = default)
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<Zone>(x => true);

            spec = spec.And(base.GuardDataAccess(spec, option));

            if (!option.IgnoreZone)
            {
                if (_executionContext.ZoneId == 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Zone>(_executionContext.FacilityId));
                }
                else
                {
                    spec = spec.And(new LimitByZoneIdSpecification<Zone>(_executionContext.ZoneId));
                }
            }

            return spec;
        }
    }
}
