using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthFacilities
{
    public class GetHealthFacilitiesByServiceTypeSpecification : ExpressionSpecification<HealthFacility>
    {
        public GetHealthFacilitiesByServiceTypeSpecification(long serviceTypeId) : base(x => x.HealthServices.Any(s => s.TypeId == serviceTypeId))
        {
        }
    }
}
