using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IHealthFacilityReadRepository : IReadRepository<HealthFacility>
    {
        Task<PagingResult<HealthFacility>> GetPagingWithFilterAsync(Pagination pagination, long facilityTypeId = 0, long branchId = 0, HealthFacilityStatus status = HealthFacilityStatus.None, bool ignoreOwner = false, CancellationToken cancellationToken = default);
    }
}
