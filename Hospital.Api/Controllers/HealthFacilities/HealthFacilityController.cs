using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthFacilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthFacilityController : CrudController<HealthFacility, HealthFacilityDto>
    {
        public HealthFacilityController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPut("add-specialty/{facilityId}/{specialtyId}")]
        public async Task<IActionResult> AddAction(long facilityId, long specialtyId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new AddSpecialtyForFacitilyCommand(facilityId, specialtyId), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
