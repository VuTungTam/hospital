using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.TimeSlots
{
    public class GetTimeSlotByTimeRuleIdSpecification : ExpressionSpecification<TimeSlot>
    {
        public GetTimeSlotByTimeRuleIdSpecification(long timeRuleId) : base(x => x.TimeRuleId == timeRuleId)
        {
            
        }
    }
}
