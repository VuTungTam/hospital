using Hospital.Application.Commands.Images;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Services.Cloud.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.UploadImages
{
    public class ImageUploadController : ApiBaseController
    {
        public ImageUploadController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file, string name)
        {

            var model = new UploadRequest(file, name);

            var command = new UploadImageCommand(model);

            var result = await _mediator.Send(command);

            return Ok(new SimpleDataResult { Data = result.Url });
        }
    }
}
