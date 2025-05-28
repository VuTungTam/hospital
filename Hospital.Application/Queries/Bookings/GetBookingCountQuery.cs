using Hospital.Application.Dtos.TimeSlots;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingCountQuery : BaseAllowAnonymousQuery<List<TimeSlotBookedDto>>
    {
        public GetBookingCountQuery(long timeRuleId, DateTime date)
        {
            Date = date;
            TimeRuleId = timeRuleId;
        }
        public long TimeRuleId { get; }
        public DateTime Date { get; }
    }
}
