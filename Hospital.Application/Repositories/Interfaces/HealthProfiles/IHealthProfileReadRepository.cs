using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthProfiles
{
    public interface IHealthProfileReadRepository : IReadRepository<HealthProfile>
    {
        Task<PaginationResult<HealthProfile>> GetPagingWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default);
    }
}
