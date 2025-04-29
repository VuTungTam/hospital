using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.ServiceTimeRules
{
    public class GetServiceTimeRuleByDayOfWeekSpecification : ExpressionSpecification<ServiceTimeRule>
    {
        public GetServiceTimeRuleByDayOfWeekSpecification(int dayOfWeek) : base(x => x.DayOfWeek == dayOfWeek)
        {
        }
    }
}
