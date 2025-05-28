using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthFacilities
{
    public class GetHealthFacilitiesByPidSpecification : ExpressionSpecification<HealthFacility>
    {
        public GetHealthFacilitiesByPidSpecification(long pid) : base(x => x.Pid == pid)
        {
        }
    }
}
