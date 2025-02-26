using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityByIdQuery : BaseAllowAnonymousQuery<HealthFacilityDto>
    {
        public GetFacilityByIdQuery(long id) 
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
