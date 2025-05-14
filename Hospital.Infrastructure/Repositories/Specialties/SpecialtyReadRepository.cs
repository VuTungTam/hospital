using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Specialties
{
    public class SpecialtyReadRepository : ReadRepository<Specialty>, ISpecialtyReadRepository
    {
        public SpecialtyReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
