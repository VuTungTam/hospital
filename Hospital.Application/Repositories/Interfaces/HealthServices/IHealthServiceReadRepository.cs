using Hospital.Application.Dtos.DoctorWorkingContexts;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IHealthServiceReadRepository : IReadRepository<HealthService>
    {
        Task<PaginationResult<HealthService>> GetPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long serviceTypeId, long facilityId, long specialtyId, long doctorId, CancellationToken cancellationToken = default);

        Task<PaginationResult<HealthService>> GetTodayPagingWithFilterAsync(Pagination pagination, HealthServiceStatus status, long serviceTypeId, long facilityId, long specialtyId, long doctorId, CancellationToken cancellationToken = default);

        Task<List<ServiceType>> GetServiceTypeByFacilityIdAsync(long facilityId, CancellationToken cancellationToken);

        Task<PaginationResult<HealthService>> GetServiceCurrentAsync(Pagination pagination, long facilityId, long doctorId, CancellationToken cancellationToken);

        Task<DoctorWorkingContextDto> GetServiceCurrentByDoctorIdAsync(long doctorId, CancellationToken cancellationToken);
    }
}
