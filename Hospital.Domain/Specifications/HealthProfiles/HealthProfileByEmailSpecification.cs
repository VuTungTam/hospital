using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthProfiles
{
    public class HealthProfileByEmailSpecification : ExpressionSpecification<HealthProfile>
    {
        public HealthProfileByEmailSpecification(string email) : base(x => x.Email == email)
        {
        }
    }
}
