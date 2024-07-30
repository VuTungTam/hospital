using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HeathServices;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VetHospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceReadRepository : ReadRepository<HealthService>, IHealthServiceReadRepository
    {
        public HealthServiceReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer) : base(serviceProvider, localizer)
        {
        }

        public virtual async Task<PagingResult<HealthService>> GetPagingByTypeAsync(Pagination pagination, long typeId, ISpecification<HealthService> spec = null, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(x => x.TypeId == typeId)
                         .BuildOrderBy(pagination.Sorts);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PagingResult<HealthService>(data, count);
        }

    }
}
