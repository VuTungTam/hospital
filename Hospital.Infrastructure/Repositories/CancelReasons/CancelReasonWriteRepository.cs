using Hospital.Application.Repositories.Interfaces.CancelReasons;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.CancelReasons
{
    public class CancelReasonWriteRepository : WriteRepository<CancelReason>, ICancelReasonWriteRepository
    {
        public CancelReasonWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}