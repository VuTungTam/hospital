using Hospital.Application.Commands.Branches;
using Hospital.Application.Dtos.Branches;
using Hospital.Application.Queries.Branches;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Branches
{
    public class BranchController : ApiBaseController
    {
        public BranchController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetBranchByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var query = new GetBranchesQuery();
            var branches = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = branches, Total = branches.Count });
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            BranchStatus status = BranchStatus.None,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetBranchesPagingQuery(pagination, status);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost("change/{id}")]
        public async Task<IActionResult> Change(long id, CancellationToken cancellationToken = default)
        {
            var command = new ChangeBranchCommand(id);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(BranchDto branch, CancellationToken cancellationToken = default)
        {
            var command = new AddBranchCommand(branch);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(BranchDto branch, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateBranchCommand(branch), cancellationToken);
            return Ok(new BaseResponse());
        }

        
    }
}
