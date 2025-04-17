using Hospital.Application.Queries.Auth.LoginHistories;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Auth
{
    public class LoginHistoryController : ApiBaseController
    {
        public LoginHistoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            long owner = 0,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size);
            var query = new GetLoginHistoriesPaginationQuery(pagination, owner);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }
    }
}
