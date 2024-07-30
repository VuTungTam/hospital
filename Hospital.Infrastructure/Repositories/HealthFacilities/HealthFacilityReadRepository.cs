using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HeathFacilities;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityReadRepository : ReadRepository<HealthFacility>, IHealthFacilityReadRepository
    {
        public HealthFacilityReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
