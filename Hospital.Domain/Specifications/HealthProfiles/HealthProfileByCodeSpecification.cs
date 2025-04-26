using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthProfiles
{
    public class HealthProfileByCodeSpecification : ExpressionSpecification<HealthProfile>
    {
        public HealthProfileByCodeSpecification(string code) : base(x => x.Code == code)
        {
        }
    }
}
