using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Specilities
{
    public class SpecialityWriteRepository : WriteRepository<Specialty>, ISpecialtyWriteRepository
    {
        public SpecialityWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
