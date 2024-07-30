using Hospital.Domain.Entities.HeathFacilities;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IHealthFacilityWriteRepository : IWriteRepository<HealthFacility>
    {
    }
}
