using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.CancelBooking)]
    public class CancelBookingCommand : BaseCommand
    {
        public CancelBookingCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
