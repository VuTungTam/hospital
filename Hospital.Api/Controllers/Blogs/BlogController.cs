using FluentValidation;
using Hospital.Application.Commands.Blogs;
using Hospital.Application.Dtos.Blogs;
using Hospital.Application.Queries.Blog.GetBlogById;
using Hospital.Application.Queries.Blog.GetBlogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hospital.Api.Controllers.Blogs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ApiBaseController
    {
        public BlogController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public virtual async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var query = new GetBlogQuery();

            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetBlogByIdQuery(id);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
        [HttpPost]
        public virtual async Task<IActionResult> Add(BlogDto blog, CancellationToken cancellationToken = default)
        {
            var command = new AddBlogCommand(blog);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpPut]
        public virtual async Task<IActionResult> Update(BlogDto blog, CancellationToken cancellationToken = default)
        {
            var command = new UpdateBlogCommand(blog);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteBlogCommand(ids);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
