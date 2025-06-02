using Hospital.Application.Dtos.TimeSlots;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingCountQuery : BaseAllowAnonymousQuery<List<TimeSlotBookedDto>>
    {
        public GetBookingCountQuery(long timeRuleId, bool isWalkin, DateTime date)
        {
            Date = date;
            TimeRuleId = timeRuleId;
            IsWalkin = isWalkin;
        }
        public long TimeRuleId { get; }
        public bool IsWalkin { get; }
        public DateTime Date { get; }
    }
}
