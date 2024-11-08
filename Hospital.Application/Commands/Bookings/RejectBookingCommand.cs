using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class RejectBookingCommand : BaseCommand
    {
        public RejectBookingCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
