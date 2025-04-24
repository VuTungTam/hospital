using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Models.Slugs;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.SharedKernel.Presentations.Controllers
{
    [ApiController]
    [Route("api/utils")]
    public class UtilsController : ControllerBase
    {
        [HttpGet("is-valid-phone/{phone}")]
        public IActionResult IsValidPhone(string phone)
        {
            return Ok(new SimpleDataResult { Data = SmsUtility.IsVietnamesePhone(phone) });
        }

        [HttpPost("to-slug")]
        public IActionResult ToSlug([FromBody] SlugConvertModel model)
        {
            return Ok(new SimpleDataResult { Data = Utility.GenerateSlug(model.Phrase ?? "") });
        }
    }
}
