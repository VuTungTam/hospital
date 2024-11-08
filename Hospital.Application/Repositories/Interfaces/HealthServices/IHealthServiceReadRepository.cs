using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IHealthServiceReadRepository : IReadRepository<HealthService>
    {
        Task<PagingResult<HealthService>> GetPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long serviceTypeId = 0, long facilityId = 0, long specialtyId = 0, bool ignoreOwner = false,  CancellationToken cancellationToken = default);
    }
}
