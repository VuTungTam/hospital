using Hospital.Application.Models.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Doctors
{
    public interface IDoctorReadRepository : IReadRepository<Doctor>
    {
        Task<PaginationResult<Doctor>> GetDoctorsPaginationResultAsync(Pagination pagination,
            FilterDoctorRequest request, AccountStatus status = AccountStatus.None,
            CancellationToken cancellationToken = default);

        Task<PaginationResult<Doctor>> GetPublicDoctorsPaginationResultAsync(Pagination pagination,
            FilterDoctorRequest request, long facilityId = 0, AccountStatus status = AccountStatus.None,
            CancellationToken cancellationToken = default);

        Task<Doctor> GetPublicDoctorById(long id, CancellationToken cancellationToken);
    }
}
