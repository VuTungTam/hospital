using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Domain.Entities.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Metas
{
    public class MetaWriteRepository : WriteRepository<Meta>, IMetaWriteRepository
    {
        public MetaWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
