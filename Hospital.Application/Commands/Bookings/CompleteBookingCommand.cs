using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class CompleteBookingCommand : BaseCommand
    {
        public CompleteBookingCommand(long id) 
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
