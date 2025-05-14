using Hospital.Application.Queries.CancelReasons;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.CancelReasons
{
    public class CancelReasonController : ApiBaseController
    {
        public CancelReasonController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("booking/{bookingId}")]
        public virtual async Task<IActionResult> GetById(long bookingId, CancellationToken cancellationToken = default)
        {
            var query = new GetCancelReasonByBookingIdQuery(bookingId);
            var cancelReason = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = cancelReason });
        }
    }
}