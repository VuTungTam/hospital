using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthProfiles
{
    public interface IHealthProfileReadRepository : IReadRepository<HealthProfile>
    {
        Task<HealthProfile> GetProfileById(long id, CancellationToken cancellationToken);

        Task<PaginationResult<HealthProfile>> GetPagingWithFilterAsync(Pagination pagination, long userId, CancellationToken cancellationToken = default);

        Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default);
    }
}
