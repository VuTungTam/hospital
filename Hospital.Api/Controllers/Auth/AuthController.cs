using Hospital.Application.Commands.Auth.Login;
using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Redis;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Auth
{

    public class AuthController : ApiBaseController
    {
        private readonly IRedisCache _redisCache;

        public AuthController(IMediator mediator, IRedisCache redisCache) : base(mediator)
        {
            _redisCache = redisCache;
        }
        [HttpGet("check-access-by-pwd/{username}"), AllowAnonymous]
        public async Task<IActionResult> CheckAccessByPassword (string username, bool autoSendOtp = true, CancellationToken cancellationToken = default)
        {
            var result = 0;
            return Ok(new SimpleDataResult { Data = result});
        }
        [HttpPost("login/with-email"), AllowAnonymous]
        public async Task<IActionResult> TraditionLogin(TraditionLoginDto dto, CancellationToken cancellationToken = default)
        {
            var command = new TraditionLoginCommand(dto, "EMAIL");
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPost("login/with-phone"), AllowAnonymous]
        public async Task<IActionResult> LoginWithPhone(TraditionLoginDto dto, CancellationToken cancellationToken = default)
        {
            var command = new TraditionLoginCommand(dto, "PHONE");
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
    }
}
