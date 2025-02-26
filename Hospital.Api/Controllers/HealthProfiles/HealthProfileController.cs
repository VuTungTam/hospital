using Hospital.Application.Commands.HealhProfiles;
using Hospital.Application.Commands.HealthProfiles;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Queries.HealthFacilities;
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

        [HttpGet("{profileId}"), AllowAnonymous]
        public async Task<IActionResult> GetProfile(long profileId, CancellationToken cancellationToken = default)
        {
            var query = new GetFacilityByIdQuery(profileId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPut]
        public virtual async Task<IActionResult> UpdateProfile(HealthProfileDto HealthProfileDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateHealthProfileCommand(HealthProfileDto);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse {  });
        }

        [HttpDelete]
        public virtual async Task<IActionResult> DeleteProfile(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteHealthProfileCommand(ids);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

    }
}
