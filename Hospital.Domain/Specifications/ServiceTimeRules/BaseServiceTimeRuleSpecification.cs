using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.ServiceTimeRules
{
    public class BaseServiceTimeRuleSpecification : ExpressionSpecification<ServiceTimeRule>
    {
        public BaseServiceTimeRuleSpecification() : base(x => true)
        {
            
        }
    }
}
