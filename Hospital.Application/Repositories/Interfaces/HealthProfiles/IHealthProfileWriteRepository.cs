using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthProfiles
{
    public interface IHealthProfileWriteRepository : IWriteRepository<HealthProfile>
    {
        Task AddProfileAsync(HealthProfile healthProfile, CancellationToken cancellationToken = default);
    }
}
