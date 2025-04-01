using Hospital.SharedKernel.Domain.Entities.Systems;

namespace Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs
{
    public interface ISystemConfigurationWriteRepository
    {
        Task SaveAsync(SystemConfiguration config, CancellationToken cancellationToken);
    }
}
