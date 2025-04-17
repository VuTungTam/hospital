using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.FacilityTypes
{
    public class GetFacilityTypeQuery : BaseAllowAnonymousQuery<List<FacilityTypeDto>>
    {
        public GetFacilityTypeQuery()
        {
            
        }
    }
}
