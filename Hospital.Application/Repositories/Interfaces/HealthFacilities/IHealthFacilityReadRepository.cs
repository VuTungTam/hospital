using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IHealthFacilityReadRepository : IReadRepository<HealthFacility>
    {
    }
}
