using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Bookings
{
    [RequiredPermission(ActionExponent.ViewBooking)]
    public class GetBookingByIdQuery : BaseQuery<BookingDto>
    {
        public GetBookingByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
