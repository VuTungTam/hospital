using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Zones
{
    public class GetZoneByFacilityIdSpecification : ExpressionSpecification<Zone>
    {
        public GetZoneByFacilityIdSpecification(long facilityId) : base(x => x.FacilityId == facilityId)
        {

        }
    }
}
