using Hospital.Application.Commands.Articles;
using Hospital.Application.Dtos.Articles;
using Hospital.Application.Queries.Articles;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Articles
{
    public class ArticleController : ApiBaseController
    {
        public ArticleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("filterable")]
        public IActionResult GetFilterable() => GetFilterable<Article>();

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption, bool vn) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<ArticleStatus>(noneOption, vn) });

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetArticleByIdQuery(id);
            var article = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = article });
        }

        [HttpGet("slug/{slug}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetBySlug(string slug, [FromQuery] List<string> langs, CancellationToken cancellationToken = default)
        {
            if (langs == null || !langs.Any())
            {
                langs = new List<string>
                {
                    "vi-VN",
                    "en-US"
                };
            }

            var query = new GetArticleBySlugQuery(slug, langs);
            var article = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = article });
        }

        [HttpGet("view-count"), AllowAnonymous]
        public virtual async Task<IActionResult> GetViewCount(long id, CancellationToken cancellationToken = default)
        {
            var viewCount = await _mediator.Send(new GetArticleViewCountQuery(id), cancellationToken);
            return Ok(new SimpleDataResult { Data = viewCount });
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            ArticleStatus status = ArticleStatus.None,
            DateTime postDate = default,
            bool clientSort = false,
            long excludeId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetArticlesPaginationQuery(pagination, status, postDate, clientSort, excludeId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(ArticleDto article, CancellationToken cancellationToken = default)
        {
            var command = new AddArticleCommand(article);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(ArticleDto article, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateArticleCommand(article), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("post")]
        public virtual async Task<IActionResult> Post(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new PostOrHiddenArticleCommand(id, true), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("hide")]
        public virtual async Task<IActionResult> Hide(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new PostOrHiddenArticleCommand(id, false), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("view-count"), AllowAnonymous]
        public virtual async Task<IActionResult> UpdateViewCount(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateArticleViewCountCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteArticleCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
