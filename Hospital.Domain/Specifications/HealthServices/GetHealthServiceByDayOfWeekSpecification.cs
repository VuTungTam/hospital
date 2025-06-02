using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesDayOfWeekSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesDayOfWeekSpecification(int dayOfWeek) : base(x => (x.ServiceTimeRules.Any(tr => tr.DayOfWeek == dayOfWeek)))
        {
        }
    }
}
