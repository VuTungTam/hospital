using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.BookingManagement)]
    public class ConfirmBookingCommand : BaseCommand
    {
        public ConfirmBookingCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
