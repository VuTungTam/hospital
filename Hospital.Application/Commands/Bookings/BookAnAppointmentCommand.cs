using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class BookAnAppointmentCommand : BaseAllowAnonymousCommand<string>
    {
        public BookAnAppointmentCommand(BookingDto booking)
        {
            Booking = booking;
        }

        public BookingDto Booking { get; }
    }
}
