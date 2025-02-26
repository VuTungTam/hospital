using Hospital.Application.Queries.Auth.Actions;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Auth
{
    public class ActionController : ApiBaseController
    {
        public ActionController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var query = new GetActionsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result, Total = result.Count });
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page = 0, int size = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetActionsPagingQuery(new Pagination(page, size));
            var result = await _mediator.Send(query, cancellationToken);    
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }
    }
}
