using Hospital.Application.Commands.Newes;
using Hospital.Application.Dtos.Newes;
using Hospital.Application.Queries.Newses;
using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Newes
{
    public class NewsController : ApiBaseController
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("filterable")]
        public IActionResult GetFilterable() => base.GetFilterable<News>();

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetNewsByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet("slug/{slug}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetBySlug(string slug, CancellationToken cancellationToken = default)
        {
            var query = new GetNewsBySlugQuery(slug);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet("paging"), AllowAnonymous]
        public async Task<IActionResult> GetPaging(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            NewsStatus status = NewsStatus.None,
            DateTime postDate = default,
            bool clientSort = false,
            long excludeId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetNewsPagingQuery(pagination, status, postDate, clientSort, excludeId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(NewsDto news, CancellationToken cancellationToken = default)
        {
            var command = new AddNewsCommand(news);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(NewsDto news, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateNewsCommand(news), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("post")]
        public virtual async Task<IActionResult> Post(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new PostOrHiddenNewsCommand(id, true), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("hide")]
        public virtual async Task<IActionResult> Hide(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new PostOrHiddenNewsCommand(id, false), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteNewsCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
