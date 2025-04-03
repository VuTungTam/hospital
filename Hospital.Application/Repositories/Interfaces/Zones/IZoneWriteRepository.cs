using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Application.Repositories.Interface;
using MediatR;

namespace Hospital.Application.Repositories.Interfaces.Zones
{
    public interface IZoneWriteRepository : IWriteRepository<Zone>
    {
        Task RemoveSpecialtiesAsync(List<ZoneSpecialty> zoneSpecialties, CancellationToken cancellationToken);
        Task UpdateZoneSpecialtiesAsync(long zoneId, IEnumerable<long> speIds, CancellationToken cancellationToken);
    }
}
