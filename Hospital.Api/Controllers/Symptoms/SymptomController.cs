using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Entities.Symptoms;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Symptoms
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomController : CrudController<Symptom, SymptomDto>
    {
        public SymptomController(IMediator mediator) : base(mediator)
        {
        }
    }
}
