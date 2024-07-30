using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Queries.HealthServices;
using Hospital.Domain.Entities.HeathServices;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthServiceController : CrudController<HealthService, HealthServiceDto>
    {
        public HealthServiceController(IMediator mediator) : base(mediator)
        {
        }
        
    }
}
