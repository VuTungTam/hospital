using Hospital.Domain.Entities.FacilityTypes;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IFacilityTypeReadRepository : IReadRepository<FacilityType>
    {
        Task<List<(FacilityType Type, int Total)>> GetTypeAsync(CancellationToken cancellationToken);
    }
}
