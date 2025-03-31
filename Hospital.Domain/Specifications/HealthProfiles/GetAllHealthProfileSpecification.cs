using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthProfiles
{
    public class GetAllHealthProfileSpecification : ExpressionSpecification<HealthProfile>
    {
        public GetAllHealthProfileSpecification() : base(x => true)
        {
        }
    }
}
