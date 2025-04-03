using Hospital.Application.Commands.Zones;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Queries.Zones;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Zones
{
    public class ZoneController : ApiBaseController
    {
        public ZoneController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("pagination"), AllowAnonymous]
        public virtual async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetZonesPaginationQuery(pagination);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetZoneByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(ZoneDto zone, CancellationToken cancellationToken = default)
        {
            var command = new AddZoneCommand(zone);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]

        public virtual async Task<IActionResult> Update(ZoneDto zone, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateZoneCommand(zone), cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteZoneCommand(ids), cancellationToken);

            return Ok(new BaseResponse());
        }
    }
}
