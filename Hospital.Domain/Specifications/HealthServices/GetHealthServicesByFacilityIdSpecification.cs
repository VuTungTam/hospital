using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesByFacilityIdSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesByFacilityIdSpecification(long facilityId) : base(x => (x.FacilityId == facilityId))
        {
        }
    }
}
