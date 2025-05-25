using Hospital.Application.Commands.Payments;
using Hospital.Application.Dtos.Payments;
using Hospital.Application.Queries.Payments;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Payments
{
    public class PaymentController : ApiBaseController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("booking/{bookingId}")]
        public virtual async Task<IActionResult> GetByBookingId(long bookingId, CancellationToken cancellationToken = default)
        {
            var query = new GetPaymentValidByBookingIdQuery(bookingId);
            var payment = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = payment });
        }

        [HttpGet("transaction/{transactionId}")]
        public virtual async Task<IActionResult> GetByTransactionId(long transactionId, CancellationToken cancellationToken = default)
        {
            var query = new GetPaymentByTransactionIdQuery(transactionId);
            var payment = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = payment });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(PaymentDto paymentDto, CancellationToken cancellationToken = default)
        {
            var command = new AddPaymentCommand(paymentDto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }
}
