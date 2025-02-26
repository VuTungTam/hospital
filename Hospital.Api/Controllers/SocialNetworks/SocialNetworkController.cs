using Hospital.Application.Commands.SocialNetworks;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Queries.SocialNerworks;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.SocialNetworks
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialNetworkController : ApiBaseController
    {
        public SocialNetworkController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet, AllowAnonymous]
        public virtual async Task<IActionResult> GetAll(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetAllSocialNetworkPagingQuery(pagination);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddSocialNetwork(
            SocialNetworkDto socialNetworkDto,
            CancellationToken cancellationToken = default)
        {
            var command = new AddSocialNetworkCommand(socialNetworkDto);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(SocialNetworkDto dto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateSocialNetworkCommand(dto), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
