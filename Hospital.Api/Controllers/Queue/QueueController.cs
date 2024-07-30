using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Queue
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ApiBaseController
    {
        public QueueController(IMediator mediator) : base(mediator)
        {

        }
    }
}
