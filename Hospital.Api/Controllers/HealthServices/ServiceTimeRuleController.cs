using Hospital.Application.Commands.ServiceTimeRules;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Queries.ServiceTimeRules;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    public class ServiceTimeRuleController : ApiBaseController
    {
        public ServiceTimeRuleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("pagination"), AllowAnonymous]
        public virtual async Task<IActionResult> GetPaging(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long serviceId = 0,
            DayOfWeek dayOfWeek = default,
            DateTime date = default,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetServiceTimeRulePagingQuery(pagination, serviceId, dayOfWeek, date);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(ServiceTimeRuleDto timeRule , CancellationToken cancellationToken = default)
        {
            var command = new AddServiceTimeRuleCommand(timeRule);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(ServiceTimeRuleDto timeRule, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateServiceTImeRuleCommand(timeRule), cancellationToken);

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
