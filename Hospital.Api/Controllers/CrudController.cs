using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController<TEntity, TDto> : ApiBaseController where TEntity : BaseEntity
    {
        public CrudController(IMediator mediator) : base(mediator)
        {
        }
    }
}
