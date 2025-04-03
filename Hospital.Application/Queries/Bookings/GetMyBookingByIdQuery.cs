using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Bookings
{
    [RequiredPermission(ActionExponent.BookingManagement)]
    public class GetMyBookingByIdQuery : BaseQuery<BookingDto>
    {
        public GetMyBookingByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
