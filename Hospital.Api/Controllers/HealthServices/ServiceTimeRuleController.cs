using Hospital.Application.Commands.ServiceTimeRules;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Queries.ServiceTimeRules;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    public class ServiceTimeRuleController : ApiBaseController
    {
        public ServiceTimeRuleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddTimeRule(ServiceTimeRuleDto timeRule , CancellationToken cancellationToken = default)
        {
            var command = new AddServiceTimeRuleCommand(timeRule);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command) });
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetTimeSlot(long serviceId, DayOfWeek dayOfWeek, CancellationToken cancellationToken)
        {
            var query = new GetTimeSlotByServiceIdQuery(dayOfWeek, serviceId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query) });
        }
    }
}
