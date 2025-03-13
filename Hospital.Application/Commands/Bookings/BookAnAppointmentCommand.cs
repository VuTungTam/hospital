using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class BookAnAppointmentCommand : BaseAllowAnonymousCommand<string>
    {
        public BookAnAppointmentCommand(BookingRequestDto booking)
        {
            Booking = booking;
        }

        public BookingRequestDto Booking { get; }
    }
}
