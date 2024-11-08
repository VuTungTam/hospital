using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthFacilities
{
    public class GetHealthFacilitiesByTypeIdSpecification : ExpressionSpecification<HealthFacility>
    {
        public GetHealthFacilitiesByTypeIdSpecification(long typeId) : base(x => (x.CategoryId == typeId))
        {
        }
    }
}
