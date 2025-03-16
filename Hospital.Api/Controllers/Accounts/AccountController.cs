using Hospital.Application.Commands.Customers;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Models.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Accounts.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Register(RegAccountRequest request, CancellationToken cancellationToken = default)
        {
            var uid = await _mediator.Send(new RegCustomerAccountCommand(request), cancellationToken);
            return Ok(new SimpleDataResult { Data = uid, Message = _localizer["account_reg_success_please_check_email"] });
        }
        [HttpPost("verify-email"), AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(VerificationAccountModel model, CancellationToken cancellationToken = default)
        {
            var command = new VerifyCustomerAccountWithEmailCommand(model);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command) });
        }

        [HttpPost("verify-phone"), AllowAnonymous]
        public async Task<IActionResult> VerifyPhone(VerificationAccountModel model, CancellationToken cancellationToken = default)
        {
            var command = new VerifyCustomerAccountWithEmailCommand(model);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command) });
        }
        [HttpPost("send-verify-email"), AllowAnonymous]
        public async Task<IActionResult> SendVerifyEmail(string email, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SendVerifyCustomerAccountWithEmailCommand(email), cancellationToken);
            return Ok(new BaseResponse { Message = _localizer["account_send_mail_success"] });
        }
    }
}
