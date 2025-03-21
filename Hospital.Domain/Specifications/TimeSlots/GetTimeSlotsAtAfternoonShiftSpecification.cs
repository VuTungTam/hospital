using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.TimeSlots
{
    public class GetTimeSlotsAtAfternoonShiftSpecification : ExpressionSpecification<TimeSlot>
    {
        public GetTimeSlotsAtAfternoonShiftSpecification() : base(x => x.Start.Hours > 12 && x.Start.Hours <= 17)
        {
            
        }
    }
}
