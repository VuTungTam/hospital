using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;
using System.Diagnostics.SymbolStore;

namespace Hospital.Infrastructure.Repositories.Symptoms
{
    public class SymptomReadRepository : ReadRepository<Symptom>, ISymptomReadRepository
    {
        public SymptomReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}
