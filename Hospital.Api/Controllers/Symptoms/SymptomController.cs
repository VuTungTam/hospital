using Hospital.Application.Commands.Symptoms;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Application.Queries.Symptoms;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Symptoms
{
    public class SymptomController : ApiBaseController
    {
        public SymptomController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("paging"), AllowAnonymous]
        public async Task<IActionResult> GetSymptomPagingGetPaging(int page, int size, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetSymptomPagingQuery(pagination);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> AddSymptom(SymptomDto symptom, CancellationToken cancellationToken = default)
        {
            var command = new AddSymptomCommand(symptom);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteSymptomCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
