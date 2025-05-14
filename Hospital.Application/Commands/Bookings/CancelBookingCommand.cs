using Hospital.Application.Models.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.CancelBooking)]
    public class CancelBookingCommand : BaseCommand
    {
        public CancelBookingCommand(CancelBookingModel model)
        {
            Model = model;

        }

        public CancelBookingModel Model { get; set; }

    }
}
