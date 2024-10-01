using Hospital.Application.Commands.Accounts.Reg;
using Hospital.Application.Dtos.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital.Api.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiBaseController
    {
        private readonly IStringLocalizer<Resources> _localizer;

        public AccountController(IMediator mediator, IStringLocalizer<Resources> localizer) : base(mediator)
        {
            _localizer = localizer;
        }
        [HttpPost("reg"), AllowAnonymous]
        public async Task<IActionResult> Register(RegAccountDto request, CancellationToken cancellationToken = default)
        {
            var uid = await _mediator.Send(new RegAccountCommand(request), cancellationToken);
            return Ok(new SimpleDataResult { Data = uid, Message = _localizer["account_reg_success_please_check_email"] });
        }
    }
}
