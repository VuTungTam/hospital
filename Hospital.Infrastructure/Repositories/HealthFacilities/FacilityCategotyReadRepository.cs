using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class FacilityCategotyReadRepository : ReadRepository<FacilityType>, IFacilityCategoryReadRepository
    {
        public FacilityCategotyReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
