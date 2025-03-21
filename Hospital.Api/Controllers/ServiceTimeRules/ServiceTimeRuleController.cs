using Hospital.Application.Commands.ServiceTimeRules;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Queries.ServiceTimeRules;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.ServiceTimeRules
{
    public class ServiceTimeRuleController : ApiBaseController
    {
        public ServiceTimeRuleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{serviceId}/pagination"), AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            long serviceId,
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            DayOfWeek? dayOfWeek = null,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetServiceTimeRulePagingQuery(pagination, serviceId, dayOfWeek);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetServiceTimeRuleByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(ServiceTimeRuleDto timeRuleDto, CancellationToken cancellationToken = default)
        {
            var command = new AddServiceTimeRuleCommand(timeRuleDto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(ServiceTimeRuleDto timeRuleDto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateServiceTimeRuleCommand(timeRuleDto), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteServiceTimeRuleCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
