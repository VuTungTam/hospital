using Hospital.Application.Queries.Blog.GetBlogById;
using Hospital.Application.Queries.Locations;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ApiBaseController
    {
        public LocationController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("provinces"), AllowAnonymous]
        public async Task<IActionResult> GetProvinces(int page = 0, int size = 20, string search = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search);
            var query = new GetProvincesPagingQuery(pagination);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("districts/{pid}")]
        public async Task<IActionResult> GetDistricts(int pid = 0, int page = 0, int size = 20, string search = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search);
            var query = new GetDistrictsPagingQuery(pagination, pid);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("wards/{did}")]
        public async Task<IActionResult> GetWards(int did = 0, int page = 0, int size = 20, string search = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search);
            var query = new GetWardsPagingQuery(pagination, did);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }
        [HttpGet("province/{name}")]
        public async Task<IActionResult> GetPid(string name, CancellationToken cancellationToken = default)
        {
            var query = new GetProvinceIdByNameQuery(name);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result});
        }

        [HttpGet("district/{name}/province/{pid}")]
        public async Task<IActionResult> GetDid(string name ,int pid, CancellationToken cancellationToken = default)
        {
            var query = new GetDistrictIdByNameQuery(name,pid);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }
        [HttpGet("ward/{name}/district/{did}")]
        public async Task<IActionResult> GetWid(string name, int did, CancellationToken cancellationToken = default)
        {
            var query = new GetWardIdByNameQuery(name, did);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }
    }
}
