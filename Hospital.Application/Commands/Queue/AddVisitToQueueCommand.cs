using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Queue
{
    public class AddBookingToQueueCommand : BaseCommand<int>
    {
        public AddBookingToQueueCommand(long bookingId) {
            BookingId = bookingId;
        }
        public long BookingId { get; set; }
    }
}
