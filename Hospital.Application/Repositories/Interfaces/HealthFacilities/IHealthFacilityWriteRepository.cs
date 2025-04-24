using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IHealthFacilityWriteRepository : IWriteRepository<HealthFacility>
    {
        Task RemoveFacilitySpecialtyAsync(FacilitySpecialty facilitySpecialty, CancellationToken cancellationToken);
        Task UpdateFacilityAsync(HealthFacility oldFacility, HealthFacilityDto newFacility, CancellationToken cancellationToken);
    }
}
