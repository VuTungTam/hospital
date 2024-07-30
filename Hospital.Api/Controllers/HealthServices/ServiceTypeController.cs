using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Queries.HealthServices;
using Hospital.Domain.Entities.HeathServices;
using Hospital.SharedKernel.Application.CQRS.Queries;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : CrudController<ServiceType, ServiceTypeDto>
    {
        public ServiceTypeController(IMediator mediator) : base(mediator)
        {
        }
    }
}
