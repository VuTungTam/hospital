using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Zones
{
    public interface IZoneReadRepository : IReadRepository<Zone>
    {
        Task<Zone> GetZoneBySpecialtyId(long specialtyId, long facilityId, CancellationToken cancellationToken);

        Task<List<Zone>> GetZonesByFacilityId(long facilityId, CancellationToken cancellationToken);
    }
}
