using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeByFacilityIdQuery : BaseAllowAnonymousQuery<List<ServiceTypeDto>>
    {
        public GetServiceTypeByFacilityIdQuery(long facilityId)
        {
            FacilityId = facilityId;
        }
        public long FacilityId { get; set; }
    }
}
