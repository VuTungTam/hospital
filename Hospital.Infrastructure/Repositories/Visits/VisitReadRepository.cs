using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Visits;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Visits
{
    public class VisitReadRepository : ReadRepository<Visit>, IVisitReadRepository
    {
        public VisitReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
