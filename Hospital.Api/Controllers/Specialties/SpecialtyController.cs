using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Queries.Specialties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Specialties
{
    public class SpecialtyController : ApiBaseController
    {
        public SpecialtyController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("paging"), AllowAnonymous]
        public async Task<IActionResult> GetSpecialtyPagingGetPaging(int page, int size, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetSpecialtyPagingQuery(pagination);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialty(SpecialtyDto specialty,CancellationToken cancellationToken = default)
        {
            var command = new AddSpecialtyCommand(specialty);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSpecialty(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteSpecialtyCommand(ids);

            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }
    }
}
