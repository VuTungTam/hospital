using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.TimeSlots
{
    public class GetTimeSlotsAtNightShiftSpecification : ExpressionSpecification<TimeSlot>
    {
        public GetTimeSlotsAtNightShiftSpecification() : base(x => x.Start.Hours > 17)
        {
            
        }
    }
}
