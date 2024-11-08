using Hospital.Application.Dtos.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using MediatR;

namespace Hospital.Api.Controllers.HealthServices
{
    public class ServiceTypeController : CrudController<ServiceType, ServiceTypeDto>
    {
        public ServiceTypeController(IMediator mediator) : base(mediator)
        {
        }
    }
}
