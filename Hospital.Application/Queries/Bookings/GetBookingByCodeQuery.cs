using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingByCodeQuery : BaseQuery<BookingDto>
    {
        public GetBookingByCodeQuery(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
