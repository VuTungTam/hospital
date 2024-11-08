using Hospital.Application.Commands.HealhProfiles;
using Hospital.Application.Commands.HealthProfiles;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Delarations
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthProfileController : ApiBaseController
    {
        public HealthProfileController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost, AllowAnonymous]
        public virtual async Task<IActionResult> AddProfile(HealthProfileDto HealthProfileDto, CancellationToken cancellationToken = default)
        {
            var command = new AddHealthProfileCommand(HealthProfileDto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> UpdateProfile(HealthProfileDto HealthProfileDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateHealthProfileCommand(HealthProfileDto);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse {  });
        }


    }
}
