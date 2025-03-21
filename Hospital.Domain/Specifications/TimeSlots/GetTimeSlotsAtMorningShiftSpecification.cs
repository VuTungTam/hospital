using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.TimeSlots
{
    public class GetTimeSlotsAtMorningShiftSpecification : ExpressionSpecification<TimeSlot>
    {
        public GetTimeSlotsAtMorningShiftSpecification() : base(x => x.Start.Hours < 12)
        {
            
        }
    }
}
