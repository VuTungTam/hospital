using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetTimeSlotByServiceIdQuery : BaseAllowAnonymousCommand<List<TimeFrame>>
    {
        public GetTimeSlotByServiceIdQuery(DayOfWeek dayOfWeek, long serviceId)
        {
            DayOfWeek = dayOfWeek;
            ServiceId = serviceId;
        }

        public DayOfWeek DayOfWeek { get; set; }
        
        public long ServiceId { get; set; }
    }
}
