using Hospital.Application.Dtos.Specialties;
using Hospital.Domain.Entities.Specialties;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Specialties
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : CrudController<Specialty, SpecialtyDto>
    {
        public SpecialtyController(IMediator mediator) : base(mediator)
        {
        }
    }
}
