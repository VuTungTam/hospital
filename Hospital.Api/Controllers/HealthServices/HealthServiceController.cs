using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Queries.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries;
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
        [HttpGet("facilityId/{facilityId}")]
        public virtual async Task<IActionResult> GetPaging(long facilityId, int page, int size, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetHealthServicePagingByFacilityQuery(pagination, facilityId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }
    }
}
