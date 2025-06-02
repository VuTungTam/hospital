using Hospital.Application.Dtos.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Application.Repositories.Interface;
using MediatR;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IHealthServiceWriteRepository : IWriteRepository<HealthService>
    {
        Task UpdateServiceAsync(HealthService oldEntity, HealthServiceDto newEntity, CancellationToken cancellationToken);
    }
}
