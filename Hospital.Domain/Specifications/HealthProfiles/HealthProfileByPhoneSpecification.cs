using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthProfiles
{
    public class HealthProfileByPhoneSpecification : ExpressionSpecification<HealthProfile>
    {
        public HealthProfileByPhoneSpecification(string phone) : base(x => x.Phone == phone)
        {
        }
    }
}
