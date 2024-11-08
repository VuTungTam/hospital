using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesByStatusSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesByStatusSpecification(HealthServiceStatus status) : base(x => (status == HealthServiceStatus.None || x.Status == status))
        {
        }
    }
}
