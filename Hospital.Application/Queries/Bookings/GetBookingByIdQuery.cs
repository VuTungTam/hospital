using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingByIdQuery : BaseQuery<BookingDto>
    {
        public GetBookingByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
