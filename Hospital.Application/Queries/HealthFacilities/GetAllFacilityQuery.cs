using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetAllFacilityQuery : BaseAllowAnonymousQuery<List<HealthFacilityDto>>
    {
        public GetAllFacilityQuery()
        {
        }
    }
}
