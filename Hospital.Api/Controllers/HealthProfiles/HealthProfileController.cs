using Hospital.Application.Commands.HealthProfiles;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Queries.HealthProfiles;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
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
        public virtual async Task<IActionResult> Add(HealthProfileDto HealthProfileDto, CancellationToken cancellationToken = default)
        {
            var command = new AddHealthProfileCommand(HealthProfileDto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetHealthProfileByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long userId = 0,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetHealthProfilePagingQuery(pagination, userId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("myself"), AllowAnonymous]
        public async Task<IActionResult> GetMyProfiles(CancellationToken cancellationToken = default)
        {
            var query = new GetMyHealthProfilesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result, Total = result.Count });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(HealthProfileDto HealthProfileDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateHealthProfileCommand(HealthProfileDto);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse { });
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteHealthProfileCommand(ids);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
