using Hospital.Application.Commands.Accounts;
using Hospital.Application.Commands.Accounts.Passwords;
using Hospital.Application.Commands.Customers;
using Hospital.Application.Commands.Doctors;
using Hospital.Application.Commands.Employees;
using Hospital.Application.Models;
using Hospital.Application.Models.Auth;
using Hospital.Application.Queries.Customers;
using Hospital.Application.Queries.Doctors;
using Hospital.Application.Queries.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Accounts.Models;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital.Api.Controllers.Accounts
{
    // [Route("api/[controller]")]
    // [ApiController]
    public class AccountController : ApiBaseController
    {
        private readonly IStringLocalizer<Resources> _localizer;
        private readonly IExecutionContext _executionContext;

        public AccountController(
            IMediator mediator,
            IStringLocalizer<Resources> localizer,
            IExecutionContext executionContext
        ) : base(mediator)
        {
            _localizer = localizer;
            _executionContext = executionContext;
        }

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<AccountStatus>(noneOption) });

        [HttpGet("profile/{includeRole}")]
        public async Task<IActionResult> GetProfile(bool includeRole = false, CancellationToken cancellationToken = default)
        {
            if (_executionContext.AccountType == AccountType.Employee)
            {
                return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetEmployeeProfileQuery(includeRole), cancellationToken) });
            }
            if (_executionContext.AccountType == AccountType.Doctor)
            {
                return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetDoctorProfileQuery(), cancellationToken) });
            }
            return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetCustomerProfileQuery(), cancellationToken) });
        }

        [HttpPost("reg"), AllowAnonymous]
        public async Task<IActionResult> Register(RegAccountRequest request, CancellationToken cancellationToken = default)
        {
            var uid = await _mediator.Send(new RegCustomerAccountCommand(request), cancellationToken);
            return Ok(new SimpleDataResult { Data = uid, Message = _localizer["Account.RegSuccessedPleaseCheckEmail"] });
        }

        [HttpPost("verify-email"), AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(VerificationAccountModel model, CancellationToken cancellationToken = default)
        {
            var command = new VerifyCustomerAccountWithEmailCommand(model);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command) });
        }

        [HttpPost("send-verify-email"), AllowAnonymous]
        public async Task<IActionResult> SendVerifyEmail(string email, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SendVerifyCustomerAccountWithEmailCommand(email), cancellationToken);
            return Ok(new BaseResponse { Message = _localizer["Account.SendEmailSuccessed"] });
        }

        [HttpPost("send-forgot-pwd-code"), AllowAnonymous]
        public async Task<IActionResult> SendForgotPwdCode(string email, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new ForgotPasswordStep1Command(email), cancellationToken);
            return Ok(new BaseResponse { Message = _localizer["Account.SendEmailSuccessed"] });
        }

        [HttpPost("verify-pwd-code"), AllowAnonymous]
        public async Task<IActionResult> VerifyPwdCode(VerifyPwdCodeModel model, CancellationToken cancellationToken = default)
        {
            var session = await _mediator.Send(new ForgotPasswordStep2Command(model), cancellationToken);
            return Ok(new SimpleDataResult { Data = session });
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatar(string fileName, CancellationToken cancellationToken = default)
        {
            var command = new UpdateAvatarCommand(fileName);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            if (_executionContext.AccountType == AccountType.Employee)
            {
                var command = new UpdateEmployeeProfileCommand(model);
                await _mediator.Send(command, cancellationToken);
            }

            else if (_executionContext.AccountType == AccountType.Doctor)
            {
                var command = new UpdateDoctorProfileCommand(model);
                await _mediator.Send(command, cancellationToken);
            }

            else
            {
                var command = new UpdateCustomerProfileCommand(model);
                await _mediator.Send(command, cancellationToken);
            }

            return Ok(new BaseResponse());
        }

        [HttpPut("change-pwd")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest dto, CancellationToken cancellationToken = default)
        {
            if (_executionContext.AccountType == AccountType.Employee)
            {
                var command = new ChangeEmployeePasswordCommand(dto);
                await _mediator.Send(command, cancellationToken);
            }

            else if (_executionContext.AccountType == AccountType.Doctor)
            {
                var command = new ChangeDoctorPasswordCommand(dto);
                await _mediator.Send(command, cancellationToken);
            }

            else
            {
                var command = new ChangeCustomerPasswordCommand(dto);
                await _mediator.Send(command, cancellationToken);
            }

            return Ok(new BaseResponse());
        }

        [HttpPut("change-pwd-by-code"), AllowAnonymous]
        public async Task<IActionResult> ChangePasswordByCode(ChangePasswordRequest dto, CancellationToken cancellationToken = default)
        {
            var command = new ForgotPasswordStep3Command(dto);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("last-seen")]
        public async Task<IActionResult> UpdateLastSeen()
        {
            await _mediator.Send(new UpdateLastSeenCommand());
            return Ok(new BaseResponse { Message = DateTime.Now.ToString() });
        }
    }
}
