using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Application.Repositories.Interface;
using MediatR;

namespace Hospital.Application.Repositories.Interfaces.HealthServices
{
    public interface IHealthServiceWriteRepository : IWriteRepository<HealthService>
    {
    }
}
