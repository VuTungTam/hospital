using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.SocialNetworks
{
    public class SocialNetworkWriteRepository : WriteRepository<SocialNetwork>, ISocialNetworkWriteRepository
    {
        public SocialNetworkWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
