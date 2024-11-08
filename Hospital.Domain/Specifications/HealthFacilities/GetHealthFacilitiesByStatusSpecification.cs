using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;
using System.Linq.Expressions;

namespace Hospital.Domain.Specifications.HealthFacilities
{
    public class GetHealthFacilitiesByStatusSpecification : ExpressionSpecification<HealthFacility>
    {
        public GetHealthFacilitiesByStatusSpecification(HealthFacilityStatus status) : base(x => (status == HealthFacilityStatus.None || x.Status == status))
        {
        }
    }
}
