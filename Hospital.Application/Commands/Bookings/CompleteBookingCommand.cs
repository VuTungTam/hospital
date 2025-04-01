using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.CompleteBooking)]

    public class CompleteBookingCommand : BaseCommand
    {
        public CompleteBookingCommand(long id) 
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
