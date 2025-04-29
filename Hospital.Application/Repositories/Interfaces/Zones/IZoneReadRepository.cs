using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Zones
{
    public interface IZoneReadRepository : IReadRepository<Zone>
    {
        Task<long> GetZoneBySpecialtyId(long specialtyId, long facilityId, CancellationToken cancellationToken);
    }
}
