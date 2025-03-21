using Hospital.Application.Dtos.TimeSlots;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetTimeSlotByIdQuery : BaseAllowAnonymousQuery<TimeSlotDto>
    {
        public GetTimeSlotByIdQuery(long id) 
        { 
            Id = id;
        }

        public long Id { get;}
    }
}
