using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Culture
{
    [Route("api/[controller]")]
    [ApiController]
    public class CultureController : ApiBaseController
    {
        public CultureController(IMediator mediator) : base(mediator)
        {

        }
    }
}
