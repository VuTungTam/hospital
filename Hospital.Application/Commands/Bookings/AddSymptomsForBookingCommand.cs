using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Bookings
{
    public class AddSymptomsForBookingCommand : BaseCommand
    {
        public AddSymptomsForBookingCommand(long bookingId, List<long> symptomIds)
        {
            BookingId = bookingId;
            SymptomIds = symptomIds;
        }
        public long BookingId { get; }
        public List<long> SymptomIds { get; }
    }
}
