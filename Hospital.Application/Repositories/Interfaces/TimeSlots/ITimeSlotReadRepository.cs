using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.TimeSlots
{
    public interface ITimeSlotReadRepository : IReadRepository<TimeSlot>
    {
        Task<List<TimeSlot>> GetByTimeRuleIdAsync(long timeRuleId, CancellationToken cancellationToken);

    }
}
