using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;
using System.Diagnostics.SymbolStore;

namespace Hospital.Infrastructure.Repositories.Symptoms
{
    public class SymptomReadRepository : ReadRepository<Symptom>, ISymptomReadRepository
    {
        public SymptomReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
