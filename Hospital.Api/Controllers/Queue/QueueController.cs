using Hospital.Application.Commands.Queue;
using Hospital.Application.Queries.Queue;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Queue
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ApiBaseController
    {
        public DateTime today = DateTime.Now;
        public QueueController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetByDay(long serviceId, [FromQuery] DateTime? created = null, CancellationToken cancellationToken = default)
        {
            var query = new GetAllQueueItemQuery(serviceId,created ?? today);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
        [HttpGet("current")]
        public virtual async Task<IActionResult> GetCurrentPossition(long serviceId, CancellationToken cancellationToken = default)
        {
            var query = new GetCurrentQueueItemQuery(serviceId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("{position}")]
        public virtual async Task<IActionResult> GetByPosition(long serviceId, int position,[FromQuery] DateTime? created = null, CancellationToken cancellationToken = default)
        {
            var query = new GetQueueItemByPositionQuery(serviceId,position, created ?? today);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
        [HttpPost]
        public virtual async Task<IActionResult> Add(long BookingId, CancellationToken cancellationToken = default)
        {
            var command = new AddBookingToQueueCommand(BookingId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpPut("complete")]
        public virtual async Task<IActionResult> FinishCurrent(long serviceId, CancellationToken cancellationToken = default)
        {
            var command = new FinishCurrentPositionCommand(serviceId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpPut("remove{position}")]
        public virtual async Task<IActionResult> RemovePosition(long serviceId, int position, CancellationToken cancellationToken = default)
        {

            var command = new RemovePositionCommand(position, today, serviceId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }
}
