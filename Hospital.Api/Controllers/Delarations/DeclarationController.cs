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
    public class DeclarationController : ApiBaseController
    {
        public DeclarationController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public virtual async Task<IActionResult> Add(DeclarationDto declarationDto, CancellationToken cancellationToken = default)
        {
            var command = new AddDeclarationCommand(declarationDto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        
    }
}
