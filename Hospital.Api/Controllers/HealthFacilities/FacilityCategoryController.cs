using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.FacilityTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthFacilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityCategoryController : CrudController<FacilityType, FacilityTypeDto>
    {
        public FacilityCategoryController(IMediator mediator) : base(mediator)
        {
        }
    }
}
