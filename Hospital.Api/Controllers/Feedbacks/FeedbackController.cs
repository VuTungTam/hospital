using Hospital.Application.Commands.Feedbacks;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Queries.Feedbacks;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Feedbacks
{
    public class FeedbackController : ApiBaseController
    {
        public FeedbackController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{id}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetFeedbackByIdQuery(id);
            var feedback = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = feedback });
        }

        [HttpGet("pagination"),AllowAnonymous]
        public async Task<IActionResult> GetFeedbackPagination(int page, int size, long serviceId, int star, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetFeedbacksPaginationQuery(pagination, star, serviceId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("myself/pagination")]
        public async Task<IActionResult> GetMyFeedbackPagination(int page, int size, long serviceId, int star, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetMyFeedbacksPaginationQuery(pagination, star, serviceId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> Add(FeedbackDto feedback, CancellationToken cancellationToken = default)
        {
            var command = new AddFeedbackCommand(feedback);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public async Task<IActionResult> Update(FeedbackDto feedback, CancellationToken cancellationToken = default)
        {
            var command = new UpdateFeedbackCommand(feedback);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }
}
