using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.HealthFacilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthFacilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityCategoryController : CrudController<FacilityCategory, FacilityCategoryDto>
    {
        public FacilityCategoryController(IMediator mediator) : base(mediator)
        {
        }
    }
}
