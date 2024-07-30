using Hospital.Domain.Entities.HeathServices;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Specifications.Interfaces;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IHealthServiceReadRepository : IReadRepository<HealthService>
    {
        Task<PagingResult<HealthService>> GetPagingByTypeAsync(Pagination pagination, long typeId, ISpecification<HealthService> spec = null, CancellationToken cancellationToken = default);
    }
}
