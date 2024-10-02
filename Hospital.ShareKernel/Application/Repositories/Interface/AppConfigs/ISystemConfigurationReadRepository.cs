using Hospital.SharedKernel.Domain.Entities.Systems;

namespace Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs
{
    public interface ISystemConfigurationReadRepository
    {
        Task<SystemConfiguration> GetAsync(CancellationToken cancellationToken = default);
    }
}
