using Hospital.SharedKernel.Application.CQRS.Commands;
using Hospital.SharedKernel.Application.CQRS.Queries;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController<TEntity, TDto> : ApiBaseController where TEntity : BaseEntity
    {
        public CrudController(IMediator mediator) : base(mediator)
        {
        }
        #region Get

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        {
            var query = new GetByIdQuery<TEntity, TDto>(id);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
        [HttpGet]
        public virtual async Task<IActionResult> GetPaging(int page, int size, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetPagingQuery<TEntity, TDto>(pagination);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }
        #endregion

        #region Post
        [HttpPost]
        public virtual async Task<IActionResult> Add(TDto dto, CancellationToken cancellationToken = default)
        {
            var command = new AddCommand<TEntity, TDto>(dto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        #endregion

        #region Put
        [HttpPut]
        public virtual async Task<IActionResult> Update(TDto dto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateCommand<TEntity, TDto>(dto), cancellationToken);
            return Ok(new BaseResponse());
        }
        #endregion

        #region Delete
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteCommand<TEntity>(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
        #endregion
    }
}
