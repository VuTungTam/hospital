using Hospital.Application.Dtos.TimeSlots;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetCurrentTimeSlotByServiceIdQuery : BaseAllowAnonymousQuery<TimeSlotDto>
    {
        public GetCurrentTimeSlotByServiceIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
