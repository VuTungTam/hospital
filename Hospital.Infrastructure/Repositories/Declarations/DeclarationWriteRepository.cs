using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Domain.Entities.Declarations;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Declarations
{
    public class DeclarationWriteRepository : WriteRepository<Declaration>, IDeclarationWriteRepository
    {
        public DeclarationWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }
    }
}
