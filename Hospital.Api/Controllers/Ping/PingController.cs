using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Ping
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet, AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }
}
