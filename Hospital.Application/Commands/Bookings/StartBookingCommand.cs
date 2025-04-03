using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.StartBooking)]
    public class StartBookingCommand : BaseCommand
    {
        public StartBookingCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}