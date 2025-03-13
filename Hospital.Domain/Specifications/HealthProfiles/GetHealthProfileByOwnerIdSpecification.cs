using Hospital.Domain.Entities.HealthProfiles;
using Hospital.SharedKernel.Specifications;
using System.Linq.Expressions;

namespace Hospital.Domain.Specifications.HealthProfiles
{
    public class GetHealthProfileByOwnerIdSpecification : ExpressionSpecification<HealthProfile>
    {
        public GetHealthProfileByOwnerIdSpecification(long ownerId) : base(x => (x.OwnerId == ownerId))
        {
        }
    }
}
