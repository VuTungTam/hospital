using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesByTypeSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesByTypeSpecification(long typeId) : base(x => (x.TypeId == typeId))
        {
        }
    }
}
