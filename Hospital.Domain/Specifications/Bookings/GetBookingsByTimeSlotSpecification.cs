using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByTimeSlotSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByTimeSlotSpecification(long timeSlotId) :  base(x => x.TimeSlotId == timeSlotId)
        {
        }
    }
}
