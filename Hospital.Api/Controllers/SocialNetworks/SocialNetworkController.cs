using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.SocialNetworks
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialNetworkController : CrudController<SocialNetwork, SocialNetworkDto>
    {
        public SocialNetworkController(IMediator mediator) : base(mediator)
        {
        }
    }
}
