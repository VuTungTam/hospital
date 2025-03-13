using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.BookingManagement)]
    public class AddBookingCommand : BaseAllowAnonymousCommand<string>
    {
        public AddBookingCommand(BookingRequestDto booking)
        {
            Booking = booking;
        }

        public BookingRequestDto Booking { get; }
    }
}
