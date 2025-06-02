using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.TimeSlots
{
    public class GetTimeSlotIsWalkinSpecification : ExpressionSpecification<TimeSlot>
    {
        public GetTimeSlotIsWalkinSpecification(bool forWalkin) : base(x => x.ForWalkin == forWalkin)
        {

        }
    }
}
