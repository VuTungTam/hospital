using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class GetMyBookingByIdQuery : BaseQuery<BookingResponseDto>
    {
        public GetMyBookingByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
