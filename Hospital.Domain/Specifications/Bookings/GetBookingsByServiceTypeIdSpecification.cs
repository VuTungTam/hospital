using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Bookings
{
    public class GetBookingsByServiceTypeIdSpecification : ExpressionSpecification<Booking>
    {
        public GetBookingsByServiceTypeIdSpecification(long serviceTypeId) : base(x => x.Service.TypeId == serviceTypeId)
        {
        }
    }
}
