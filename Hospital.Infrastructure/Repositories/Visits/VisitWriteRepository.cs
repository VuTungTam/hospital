using Hospital.Application.Repositories.Interfaces.Visits;
using Hospital.Domain.Entities.Visits;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Visits
{
    public class VisitWriteRepository : WriteRepository<Visit>, IVisitWriteRepository
    {
        public VisitWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
