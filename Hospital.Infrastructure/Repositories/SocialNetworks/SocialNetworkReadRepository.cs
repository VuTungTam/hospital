using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.SocialNetworks
{
    public class SocialNetworkReadRepository : ReadRepository<SocialNetwork>, ISocialNetworkReadRepository
    {
        public SocialNetworkReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
