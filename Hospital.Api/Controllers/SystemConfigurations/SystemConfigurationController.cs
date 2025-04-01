using Hospital.Application.Commands.SystemConfigurations;
using Hospital.Application.Dtos.SystemConfigurations;
using Hospital.Domain.Configs;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Hospital.Application.Queries.SystemConfigurations;

namespace Hospital.Api.Controllers.SystemConfigurations
{
    public class SystemConfigurationController : ApiBaseController
    {

        public SystemConfigurationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var query = new GetSystemConfigurationQuery();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpPut("save")]
        public virtual async Task<IActionResult> Update(SystemConfigurationDto config, CancellationToken cancellationToken = default)
        {
            if (!FeatureConfig.SystemSetting)
            {
                throw new ForbiddenException();
            }

            await _mediator.Send(new UpdateSystemConfigurationCommand(config), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
