using Hospital.Application.Commands.Blogs;
using Hospital.Application.Commands.Declarations;
using Hospital.Application.Dtos.Blogs;
using Hospital.Application.Dtos.Declarations;
using Hospital.Domain.Entities.Declarations;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Delarations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDeclarationRequest
    {
        public DeclarationDto Declaration { get; set; }
        public List<long> SymptomIds { get; set; }
    }
    public class DeclarationController : ApiBaseController
    {
        public DeclarationController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] AddDeclarationRequest request, CancellationToken cancellationToken = default)
        {
            var command = new AddDeclarationCommand(request.Declaration, request.SymptomIds);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }
}
