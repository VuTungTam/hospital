using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.HeathFacilities;
using MediatR;
using Microsoft.AspNetCore.Http;
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
