namespace Hospital.Domain.Entities.TimeSlots
{
    public class TimeSlotEqualityComparer : IEqualityComparer<TimeSlot>
    {
        public bool Equals(TimeSlot x, TimeSlot y)
        {
            if (x == null || y == null) return false;
            return x.Start == y.Start && x.End == y.End && x.TimeRuleId == y.TimeRuleId;
        }

        public int GetHashCode(TimeSlot obj)
        {
            return HashCode.Combine(obj.Start, obj.End, obj.TimeRuleId);
        }
    }
}
