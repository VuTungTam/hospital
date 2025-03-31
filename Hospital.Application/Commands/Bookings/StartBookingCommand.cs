using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class StartBookingCommand : BaseCommand
    {
        public StartBookingCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
