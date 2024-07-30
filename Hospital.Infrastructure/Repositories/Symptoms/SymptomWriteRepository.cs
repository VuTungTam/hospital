using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Symptoms
{
    public class SymptomWriteRepository : WriteRepository<Symptom>, ISymptomWriteRepository
    {
        public SymptomWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
