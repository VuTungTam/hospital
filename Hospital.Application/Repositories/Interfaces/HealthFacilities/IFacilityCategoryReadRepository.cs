using Hospital.Domain.Entities.FacilityTypes;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.HealthFacilities
{
    public interface IFacilityCategoryReadRepository : IReadRepository<FacilityType>
    {
    }
}
