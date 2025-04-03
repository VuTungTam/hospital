using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.BookingAppointment)]
    public class BookAnAppointmentCommand : BaseAllowAnonymousCommand<string>
    {
        public BookAnAppointmentCommand(BookingDto booking)
        {
            Booking = booking;
        }

        public BookingDto Booking { get; }
    }
}
