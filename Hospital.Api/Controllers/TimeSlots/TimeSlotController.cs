using Hospital.Application.Queries.TimeSlots;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.TimeSlots
{
    public class TimeSlotController : ApiBaseController
    {
        public TimeSlotController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{timeRuleId}/pagination"), AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            long timeRuleId,
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetTimeSlotsPaginationQuery(pagination, timeRuleId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetTimeSlotByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }
    }
}
