using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IServiceTypeReadRepository : IReadRepository<ServiceType>
    {
        Task<ServiceType> GetTypeBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken);
    }
}
