using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetMostFacilityQuery : BaseAllowAnonymousQuery<List<HealthFacilityDto>>
    {
        public GetMostFacilityQuery()
        {
        }
    }
}
