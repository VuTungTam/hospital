using Hospital.Application.Commands.Notifications;
using Hospital.Application.Queries.Notifications;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Notifications
{
    public class NotificationController : ApiBaseController
    {
        public NotificationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken = default)
        {
            var query = new GetNotificationsCountQuery();
            var count = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = count });
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination(int page = 0, int size = 20, string search = "", CancellationToken cancellationToken = default)
        {
            var query = new GetNotificationsPaginationQuery(new Pagination(page, size, search));
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPut("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken = default)
        {
            var command = new MarkAllNotificationAsReadCommand();
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpPut("mark/{id}/{markAsRead}")]
        public async Task<IActionResult> MarkAsRead(long id, bool markAsRead, CancellationToken cancellationToken = default)
        {
            var command = new MarkNotificationCommand(id, markAsRead);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteNotificationsCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
