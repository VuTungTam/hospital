using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.TimeSlots
{
    public interface ITimeSlotWriteRepository : IWriteRepository<TimeSlot>
    {
    }
}
