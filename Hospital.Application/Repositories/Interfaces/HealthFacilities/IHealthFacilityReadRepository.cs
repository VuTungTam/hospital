using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IHealthFacilityReadRepository : IReadRepository<HealthFacility>
    {
        Task<PaginationResult<HealthFacility>> GetPagingWithFilterAsync(Pagination pagination, long facilityTypeId = 0, long serviceTypeId = 0, HealthFacilityStatus status = HealthFacilityStatus.None, CancellationToken cancellationToken = default);

        Task<HealthFacility> GetBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken);

        Task<PaginationResult<HealthFacility>> GetNamePaginationAsync(Pagination pagination, CancellationToken cancellationToken = default);
    }
}
