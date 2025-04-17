using Hospital.Application.Commands.Metas;
using Hospital.Application.Dtos.Metas;
using Hospital.Application.Queries.Metas;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Metas
{
    public class MetaController : ApiBaseController
    {
        public MetaController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("injector"), AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetScriptQuery(), cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet("tags"), AllowAnonymous]
        public async Task<IActionResult> GetTags(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetMetaTagsQuery(), cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPut("update-script")]
        public async Task<IActionResult> UpdateScript(ScriptDto script, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateScriptCommand(script), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("upsert-meta")]
        public async Task<IActionResult> UpsertMeta(MetaDto meta, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpsertMetaTagCommand(meta), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
