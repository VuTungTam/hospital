using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.ServiceTimeRules
{
    public class GetServiceTimeRuleByServiceIdSpecification : ExpressionSpecification<ServiceTimeRule>
    {
        public GetServiceTimeRuleByServiceIdSpecification(long serviceId) : base(x => x.ServiceId == serviceId)
        {
        }
    }
}
