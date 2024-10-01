using Hospital.Application.Commands.Visits;
using Hospital.Application.Dtos.Visits;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Visits
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ApiBaseController
    {
        public VisitController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public virtual async Task<IActionResult> AddVisit(VisitDto dto, CancellationToken cancellationToken = default)
        {
            var command = new AddVisitCommand(dto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpPost("add-symtom/{visitId}")]
        public virtual async Task<IActionResult> AddSymptom([FromRoute] long visitId, [FromBody] List<long> symptomIds, CancellationToken cancellationToken = default)
        {
            var command = new AddSymptomForVisitCommand(visitId, symptomIds);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }

}
