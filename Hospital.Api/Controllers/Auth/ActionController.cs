using Hospital.Application.Queries.Auth.Actions;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
