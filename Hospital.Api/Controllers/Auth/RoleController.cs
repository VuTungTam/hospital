using Hospital.Application.Commands.Auth.Roles;
using Hospital.Application.Queries.Auth.Roles;
using Hospital.Domain.Configs;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital.Api.Controllers.Auth
{
    public class RoleController : ApiBaseController
    {
        private readonly IStringLocalizer<Resources> _localizer;
        public RoleController(IMediator mediator, IStringLocalizer<Resources> localizer) : base(mediator)
        {
            _localizer = localizer;
        }

        [HttpGet("entire")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var query = new GetRolesQuery();
            var data = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = data, Total = data.Count });
        }

        [HttpPut("add-action/{roleId}/{actionId}")]
        public async Task<IActionResult> AddAction(long roleId, long actionId, CancellationToken cancellationToken = default)
        {
            if (!FeatureConfig.Decentralization)
            {
                throw new ForbiddenException();
            }

            await _mediator.Send(new AddActionForRoleCommand(roleId, actionId), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete("remove-action/{roleId}/{actionId}")]
        public async Task<IActionResult> RemoveAction(long roleId, long actionId, CancellationToken cancellationToken = default)
        {
            if (!FeatureConfig.Decentralization)
            {
                throw new ForbiddenException();
            }

            await _mediator.Send(new RemoveActionForRoleCommand(roleId, actionId), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
