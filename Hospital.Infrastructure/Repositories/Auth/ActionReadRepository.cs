using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Domain.Constants;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Action = Hospital.SharedKernel.Application.Services.Auth.Entities.Action;
namespace Hospital.Infrastructure.Repositories.Auth
{
    public class ActionReadRepository : ReadRepository<Action>, IActionReadRepository
    {
        public ActionReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<Action> GetMasterAsync(CancellationToken cancellationToken)
        {
            var key = BaseCacheKeys.GetMasterActionKey();
            var valueFactory = () => _dbSet.FirstOrDefaultAsync(x => x.Code == "master", cancellationToken);

            return await _redisCache.GetOrSetAsync(key, valueFactory, TimeSpan.FromSeconds(AppCacheTime.MasterAction), cancellationToken);
        }
    }
}
